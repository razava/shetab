using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProcessRepository : GenericRepository<Process>, IProcessRepository
{
    private readonly IUserRepository _userRepository;
    public ProcessRepository(ApplicationDbContext dbContext, IUserRepository userRepository) : base(dbContext)
    {
        _userRepository = userRepository;
    }

    public async Task<Process?> GetByIDAsync(int id, bool trackChanges = true)
    {
        var includedStage = @"Stages,Stages.Actors,Stages.Actors.Regions,Stages.Actors.BotActor,Stages.Actors.BotActor.Transition,Stages.Actors.BotActor.DestinationActor";
        var includedTransition = @"Transitions,Transitions.From,Transitions.To,Transitions.ReasonList";
        var includedRevisionUnit = @"RevisionUnit";
        var result = await base.GetSingleAsync(p => p.Id == id, trackChanges, $"{includedStage},{includedTransition},{includedRevisionUnit}");

        return result;
    }

    public async Task<Process?> AddTypicalProcess(int instanceId, string code, string title, List<int> actorIds)
    {
        Process process;
        ProcessStage stageCitizen, stageOperator, stageExecutive, stageContractor, stageInspector;
        List<ProcessReason> ReasonList;
        ProcessTransition transition12, transition21;
        BotActor bot12, bot21;
        Actor? actorCitizenRole;
        Actor? actorOperatorRole;
        Actor? actorExecutiveRole;
        Actor? actorContractorRole;
        Actor? actorInspectorRole;

        var roleId = (await _userRepository.FindRoleByNameAsync("Citizen"))?.Id;
        if (roleId is null) throw new Exception("Role cannot be null here.");
        actorCitizenRole = await context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindRoleByNameAsync("Operator"))?.Id;
        if (roleId is null) throw new Exception("Role cannot be null here.");
        actorOperatorRole = await context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindRoleByNameAsync("Executive"))?.Id;
        if (roleId is null) throw new Exception("Role cannot be null here.");
        actorExecutiveRole = await context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindRoleByNameAsync("Contractor"))?.Id;
        if (roleId is null) throw new Exception("Role cannot be null here.");
        actorContractorRole = await context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        roleId = (await _userRepository.FindRoleByNameAsync("Inspector"))?.Id;
        if (roleId is null) throw new Exception("Role cannot be null here.");
        actorInspectorRole = await context.Set<Actor>()
            .Where(p => p.Type == ActorType.Role && p.Identifier == roleId)
            .FirstOrDefaultAsync();

        if (actorCitizenRole is null
            || actorOperatorRole is null
            || actorExecutiveRole is null
            || actorContractorRole is null
            || actorInspectorRole is null)
            throw new Exception("");

        //Create process related to an executive
        process = new Process()
        {
            ShahrbinInstanceId = instanceId,
            Title = "فرآیند (" + title + ")",
            Code = code
        };

        stageCitizen = new ProcessStage()
        {
            Name = "Citizen",
            DisplayName = "شهروند",
            Order = 1,
            Status = "ثبت درخواست در سامانه شهربین",
            DisplayRoleId = actorCitizenRole.Identifier,
            Actors = new List<Actor>() { actorCitizenRole }
        };
        process.Stages.Add(stageCitizen);


        stageOperator = new ProcessStage()
        {
            Name = "Operator",
            DisplayName = "اپراتور",
            Order = 10,
            Status = "در انتظار تأیید اپراتور",
            DisplayRoleId = actorOperatorRole.Identifier,
            Actors = new List<Actor>() { actorOperatorRole }
        };
        process.Stages.Add(stageOperator);

        var actors = await context.Set<Actor>().Where(p => actorIds.Contains(p.Id)).ToListAsync();

        stageExecutive = new ProcessStage()
        {
            Name = "Executive",
            DisplayName = "واحد اجرایی",
            Order = 20,
            Status = "ارجاع به واحد اجرایی",
            DisplayRoleId = actorExecutiveRole.Identifier,
            Actors = actors
        };
        process.Stages.Add(stageExecutive);


        stageContractor = new ProcessStage()
        {
            Name = "Contractor",
            DisplayName = "پیمانکار",
            Order = 30,
            Status = "ارجاع به پیمانکار",
            DisplayRoleId = actorContractorRole.Identifier,
            Actors = new List<Actor>() { actorContractorRole }
        };
        process.Stages.Add(stageContractor);


        stageInspector = new ProcessStage()
        {
            Name = "Inspector",
            DisplayName = "واحد بازرسی",
            Order = 40,
            Status = "ارجاع به واحد بازرسی",
            DisplayRoleId = actorInspectorRole.Identifier,
            Actors = new List<Actor>() { actorInspectorRole }
        };
        process.Stages.Add(stageInspector);


        ////////////////////////////
        ///
        ReasonList = new List<ProcessReason>()
                    {
                        new ProcessReason()
                        {
                            Title = "پیش فرض",
                            Description = "توضیحات",
                            ReasonMeaning = ReasonMeaning.Ok
                        }
                    };
        transition12 = new ProcessTransition()
        {
            From = stageCitizen,
            To = stageOperator,
            Message = "ارجاع به اپراتور",
            Title = "1-2",
            ReasonList = ReasonList,
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 1
        };

        process.Transitions.Add(transition12);

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageOperator,
            To = stageExecutive,
            Message = "ارجاع به واحد اجرایی",
            Title = "2-3",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "جهت بررسی", ReasonMeaning = ReasonMeaning.Ok }
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 1
        });

        transition21 = new ProcessTransition()
        {
            From = stageOperator,
            To = stageCitizen,
            Message = "ارجاع به شهروند",
            Title = "2-1",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "رسیدگی شد", ReasonMeaning = ReasonMeaning.Ok },
                            new ProcessReason(){ Title = "رد شد", ReasonMeaning = ReasonMeaning.Nok }
                        },
            ReportState = ReportState.Finished,
            CanSendMessageToCitizen = false,
            Order = 2
        };
        process.Transitions.Add(transition21);

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageExecutive,
            To = stageOperator,
            Message = "ارجاع به اپراتور",
            Title = "3-2",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "رسیدگی شد", ReasonMeaning = ReasonMeaning.Ok },
                            new ProcessReason(){ Title = "خارج از حوزه", ReasonMeaning = ReasonMeaning.Nok },
                            new ProcessReason(){ Title = "رد شد", ReasonMeaning = ReasonMeaning.Nok }
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = true,
            Order = 2
        });


        process.Transitions.Add(new ProcessTransition()
        {
            From = stageExecutive,
            To = stageContractor,
            Message = "ارجاع به پیمانکار",
            Title = "3-4",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "جهت اجرا", ReasonMeaning = ReasonMeaning.Ok }
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = true,
            Order = 1
        });

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageContractor,
            To = stageExecutive,
            Message = "ارجاع به واحد اجرایی",
            Title = "4-3",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "انجام شد", ReasonMeaning = ReasonMeaning.Ok },
                            new ProcessReason(){ Title = "امکانپذیر نیست", ReasonMeaning = ReasonMeaning.Nok },

                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 1
        });

        process.Transitions.Add(new ProcessTransition()
        {
            From = stageCitizen,
            To = stageInspector,
            Message = "ارجاع به واحد بازرسی",
            Title = "1-5",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "عدم رضایت از نحوه رسیدگی", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Review,
            CanSendMessageToCitizen = false,
            TransitionType = TransitionType.Objection,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageCitizen,
            Message = "ارجاع به شهروند",
            Title = "5-1",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "اعتراض وارد نیست", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Accepted,
            CanSendMessageToCitizen = false,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageOperator,
            Message = "ارجاع به اپراتور",
            Title = "5-2",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "بررسی مجدد", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageExecutive,
            Message = "ارجاع به واحد اجرایی",
            Title = "5-3",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "بررسی مجدد", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 4
        });
        process.Transitions.Add(new ProcessTransition()
        {
            From = stageInspector,
            To = stageContractor,
            Message = "ارجاع به پیمانکار",
            Title = "5-4",
            ReasonList = new List<ProcessReason>()
                        {
                            new ProcessReason(){ Title = "بررسی مجدد", ReasonMeaning = ReasonMeaning.Nok },
                        },
            ReportState = ReportState.Live,
            CanSendMessageToCitizen = false,
            Order = 4
        });

        ////////////////
        bot12 = new BotActor()
        {
            Transition = transition12,
            DestinationActor = actorOperatorRole,
            MessageToCitizen = "",
            Priority = Priority.Normal,
            ReasonId = null,
            Visibility = Visibility.Operators
        };
        context.Set<BotActor>().Add(bot12);

        stageCitizen.Actors.Add(new Actor()
        {
            Identifier = bot12.Id,
            BotActorId = bot12.Id,
            Type = ActorType.Auto
        });

        bot21 = new BotActor()
        {
            Transition = transition21,
            DestinationActor = actorCitizenRole,
            MessageToCitizen = "",
            Priority = null,
            ReasonId = null,
            Visibility = null,
            ReasonMeaning = ReasonMeaning.Ok,
        };
        context.Set<BotActor>().Add(bot21);

        stageOperator.Actors.Add(new Actor()
        {
            Identifier = bot21.Id,
            BotActorId = bot21.Id,
            Type = ActorType.Auto
        });

        ///////////////////
        ///
        context.Set<Process>().Add(process);
        //await context.SaveChangesAsync();

        return process;
    }

    public async Task<Process?> UpdateTypicalProcess(int id, string code, string title, List<int> actorIds)
    {
        Process? process;
        ProcessStage? stageExecutive;


        //Create process related to an executive
        process = await context.Set<Process>()
            .Where(p => p.Id == id)
            .Include(p => p.Stages)
            .ThenInclude(p => p.Actors)
            .SingleOrDefaultAsync();
        if (process is null)
            throw new Exception("Process cannot be null here.");
        process.Title = title;
        process.Code = code;

        var actors = await context.Set<Actor>().Where(p => actorIds.Contains(p.Id)).ToListAsync();

        stageExecutive = process.Stages.Where(p => p.Name == "Executive").FirstOrDefault();
        if (stageExecutive is null)
            throw new Exception("Stage cannot be null here.");
        stageExecutive.Actors = actors;


        ///////////////////
        ///
        context.Entry(process).State = EntityState.Modified;
        //await context.SaveChangesAsync();
        return process;
    }
}
