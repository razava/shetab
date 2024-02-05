using System.ComponentModel.DataAnnotations.Schema;
using Domain.Exceptions;
using Domain.Messages;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Domain.Primitives;

namespace Domain.Models.Relational;

public class Report : Entity
{
    public int ShahrbinInstanceId { get; private set; }
    public ShahrbinInstance ShahrbinInstance { get; private set; } = null!;
    public DateTime Sent { get; private set; }
    public DateTime? Finished { get; private set; }
    public DateTime? Responsed { get; private set; }
    public DateTime Deadline { get; private set; }
    public DateTime? ResponseDeadline { get; private set; }
    public DateTime LastStatusDateTime { get; private set; }
    public double? Duration { get; private set; }
    public double? ResponseDuration { get; private set; }

    public int CategoryId { get; private set; }
    public Category Category { get; set; } = null!;
    public Address Address { get; private set; } = null!;
    public string TrackingNumber { get; private set; } = null!;
    public Priority Priority { get; private set; }
    public Visibility Visibility { get; private set; }
    public ICollection<Media> Medias { get; private set; } = new List<Media>();
    public string Comments { get; private set; } = string.Empty;
    public ReportState ReportState { get; private set; }
    public string LastStatus { get; private set; } = string.Empty;

    //Process infos
    public int ProcessId { get; private set; }
    public Process Process { get; set; } = null!;
    public int? CurrentStageId { get; private set; }
    public ProcessStage? CurrentStage { get; private set; }
    public int? LastTransitionId { get; private set; }
    public ProcessTransition? LastTransition { get; private set; }
    public int? LastReasonId { get; private set; }
    public ProcessReason? LastReason { get; private set; }
    //public string CurrentActorsStr { get; private set; } = string.Empty;
    //public ICollection<Actor> CurrentActors { get; private set; } = new List<Actor>();
    public int? CurrentActorId { get; set; }
    public Actor? CurrentActor { get; set; }
    public ICollection<TransitionLog> TransitionLogs { get; private set; } = new List<TransitionLog>();


    //People
    public string CitizenId { get; private set; } = string.Empty;
    [ForeignKey("CitizenId")]
    public ApplicationUser Citizen { get; private set; } = null!;
    public string? RegistrantId { get; private set; }
    [ForeignKey("RegistrantId")]
    public ApplicationUser? Registrant { get; private set; }
    public string? ExecutiveId { get; private set; }
    [ForeignKey("ExecutiveId")]
    public ApplicationUser? Executive { get; private set; }
    public string? ContractorId { get; private set; }
    [ForeignKey("ContractorId")]
    public ApplicationUser? Contractor { get; private set; }
    public string? InspectorId { get; private set; }
    [ForeignKey("InspectorId")]
    public ApplicationUser? Inspector { get; private set; }

    //Social
    [InverseProperty("ReportsLiked")]
    public ICollection<ApplicationUser> LikedBy { get; private set; } = new List<ApplicationUser>();
    public int Likes { get; private set; }
    public ICollection<Comment> FeedbackComments { get; private set; } = new List<Comment>();
    public int CommentsCount { get; private set; }
    public ICollection<Violation> Violations { get; private set; } = new List<Violation>();
    public ICollection<Message> Messages { get; private set; } = new List<Message>();
    public Guid? FeedbackId { get; private set; }
    public Feedback? Feedback { get; private set; }
    public int? Rating { get; private set; }


    //Status
    public ReportFlags Flags { get; private set; }
    public bool IsIdentityVisible { get; private set; }
    public bool IsObjectioned { get; private set; }
    public bool IsFeedbacked { get; private set; }


    #region Constructors
    private Report(Guid id) : base(id) { }
    private Report(
        Guid id,
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        List<Media> attachments,
        Visibility visibility = Visibility.EveryOne,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true) : base(id)
    {
        var now = DateTime.UtcNow;

        ShahrbinInstanceId = category.ShahrbinInstanceId;
        CitizenId = citizenId;
        CategoryId = category.Id;
        ProcessId = category.ProcessId ?? throw new CategoryHaveNoAssignedProcess();
        Process = category.Process ?? throw new NotLoadedProcessException();
        Comments = comments;
        Medias = attachments;
        IsIdentityVisible = isIdentityVisible;
        Address = address;
        Sent = now;
        //TODO: Update report state to contain more info and use ToString to get description instead of LastStatus been stored in database.
        LastStatusDateTime = now;
        Deadline = now.AddHours(category.Duration);
        ResponseDeadline = category.ResponseDuration == null ? null : now.AddHours(category.ResponseDuration.Value);
        Visibility = visibility;
        TrackingNumber = generateTrackingNumber(phoneNumber);
        Priority = priority;
    }
    #endregion

    #region Factory methods
    public static Report NewByCitizen(
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        List<Media> attachments,
        Visibility visibility,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true)
    {
        var report = new Report(
            Guid.NewGuid(),
            citizenId,
            phoneNumber,
            category,
            comments,
            address,
            attachments,
            visibility,
            priority,
            isIdentityVisible);

        report.ReportState = ReportState.NeedAcceptance;
        report.LastStatus = ReportMessages.NeedAcceptance;

        if (!category.EditingAllowed)
        {
            report.InitProcess();
        }

        var log = TransitionLog.CreateNewReport(report.Id, ActorType.Person, citizenId);

        report.TransitionLogs.Add(log);

        var message =
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = ReportMessages.Created,
                DateTime = report.Sent,
                MessageType = MessageType.Report,
                SubjectId = report.Id,
                Recepients = new List<MessageRecepient>()
                {
                    new MessageRecepient() { Type = RecepientType.Person, ToId = report.CitizenId }
                }
            };
        report.Messages.Add(message);

        return report;
    }

    public static Report NewByOperator(
        string operatorId,
        string citizenId,
        string phoneNumber,
        Category category,
        string comments,
        Address address,
        List<Media> attachments,
        Visibility visibility,
        Priority priority = Priority.Normal,
        bool isIdentityVisible = true)
    {
        var now = DateTime.UtcNow;

        var report = new Report(
            Guid.NewGuid(),
            citizenId,
            phoneNumber,
            category,
            comments,
            address,
            attachments,
            visibility,
            priority,
            isIdentityVisible);

        report.RegistrantId = operatorId;

        var log = TransitionLog.CreateNewReport(report.Id, ActorType.Person, operatorId);
        
        report.TransitionLogs.Add(log);
        
        report.InitProcess();

        var message =
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = ReportMessages.Created,
                DateTime = report.Sent,
                MessageType = MessageType.Report,
                SubjectId = report.Id,
                Recepients = new List<MessageRecepient>()
                {
                    new MessageRecepient() { Type = RecepientType.Person, ToId = report.CitizenId }
                }
            };
        report.ReportState = ReportState.Live;
        report.LastStatus = "ثبت درخواست در سامانه";
        report.Messages.Add(message);

        return report;
    }
    #endregion

    #region PublicMethods
    public void InitProcess()
    {
        var now = DateTime.UtcNow;
        var firstStage = Process.Stages.OrderBy(p => p.Id).First();
        CurrentStageId = firstStage.Id;
        LastStatus = firstStage.Status;
        LastStatusDateTime = now;
        autoTransition();
    }

    public void Accept(
        string operatorId,
        Category? category,
        string? comments,
        Address? address,
        List<Media>? attachments,
        Visibility? visibility)
    {
        updateReport(category, comments, address, attachments, visibility);
        ReportState = ReportState.Live;
        LastStatus = "تأیید درخواست در سامانه";
        LastStatusDateTime = DateTime.UtcNow;
        Visibility = Visibility.Operators;
        Priority = Priority.Normal;
        InitProcess();
        var log = TransitionLog.CreateApproved(Id, operatorId);

        TransitionLogs.Add(log);
    }

    public void Update(
        string operatorId,
        Category? category,
        string? comments,
        Address? address,
        List<Media>? attachments,
        Visibility? visibility)
    {
        updateReport(category, comments, address, attachments, visibility);
        //TODO: Log modified values
        var comment = "";
        var log = TransitionLog.CreateUpdate(Id, comment ?? "", operatorId);

        TransitionLogs.Add(log);
    }

    public Message MessageToCitizen(
        string actorIdentifier,
        List<Media> attachments,
        string message,
        string comment)
    {
        var now = DateTime.UtcNow;

        Responsed = now;
        ResponseDuration = (now - Sent).TotalSeconds;

        var log = TransitionLog.CreateMessageToCitizen(Id, comment, attachments, actorIdentifier, ResponseDuration ?? 0);

        TransitionLogs.Add(log);


        var resultMessage = new Message()
        {
            ShahrbinInstanceId = ShahrbinInstanceId,
            Title = "پاسخ به شهروند" + " - " + TrackingNumber,
            Content = message,
            DateTime = now,
            MessageType = MessageType.Report,
            SubjectId = Id,
            Recepients = new List<MessageRecepient>()
                {
                        new MessageRecepient() { Type = RecepientType.Person, ToId = CitizenId }
                }
        };

        Messages.Add(resultMessage);

        return resultMessage;
    }

    public void MakeTransition(
        int transitionId,
        int reasonId,
        List<Media> attachments,
        string comment,
        ActorType actorType,
        string actorIdentifier,
        int toActorId,
        List<Actor> actors,
        bool isExecutive = false,
        bool isContractor = false)
    {
        makeTransition(
            transitionId,
            reasonId,
            attachments,
            comment,
            actorType,
            actorIdentifier,
            toActorId,
            actors,
            isExecutive,
            isContractor);
    }

    public void MoveToStage(
        bool isAccepted,
        int stageId,
        int toActorId,
        string comment,
        List<Media> attachments,
        string inspectorId,
        Visibility? visibility)
    {
        moveToStage(isAccepted, stageId, toActorId, comment, attachments, inspectorId, visibility);
    }

    public List<ProcessTransition> GetPossibleTransitions()
    {
        var possibleTransitions = Process.Transitions
            .Where(p => p.FromId == CurrentStageId)
            .OrderBy(p => p.Order)
            .ToList();

        return possibleTransitions;
    }
    #endregion

    public void UpdateFeedback(int rating)
    {
        IsFeedbacked = true;
        Rating = rating;

        TransitionLogs.Add(TransitionLog.CreateFeedback(Id, CitizenId, $"ثبت بازخورد شهروند با امتیاز {rating}"));
    }

    #region Private methods
    private void updateReport(
        Category? category,
        string? comments,
        Address? address,
        List<Media>? attachments,
        Visibility? visibility)
    {
        var now = DateTime.UtcNow;

        if (category is not null)
        {
            CategoryId = category.Id;
            Category = category;
            Deadline = now.AddHours(category.Duration);
            ResponseDeadline = category.ResponseDuration == null ? null : now.AddHours(category.ResponseDuration.Value);
            InitProcess();
        }

        Comments = comments ?? Comments;
        Address = address ?? Address;
        Visibility = visibility ?? Visibility;
        if (attachments != null)
        {
            Medias = attachments;
        }
            

        return;
    }

    private string generateTrackingNumber(string userName)
    {
        Random random = new Random();
        var result = userName.Substring(userName.Length - 4);
        result += random.Next(111111, 999999).ToString();

        return result;
    }

    private void autoTransition()
    {
        if (ReportState == ReportState.Finished || ReportState == ReportState.Accepted)
            return;

        var currentStage = Process.Stages.FirstOrDefault(p => p.Id == CurrentStageId);
        if (currentStage == null)
            throw new NullCurrentStageException();


        var autoActors = currentStage.Actors.Where(p => p.Type == ActorType.Auto).ToList();
        foreach (var autoActor in autoActors)
        {
            //Revise this
            var bot = autoActor.BotActor;
            if (bot == null)
                throw new BotNotFoundException();

            if (bot.ReasonMeaning == null || LastReason != null && LastReason.ReasonMeaning == bot.ReasonMeaning)
            {
                makeTransition(
                    bot.TransitionId,
                    bot.ReasonId,
                    new List<Media>(),
                    "",
                    ActorType.Auto,
                    bot.Id,
                    bot.DestinationActorId,
                    new List<Actor> { autoActor });
                break;
            }
        }
    }

    private void makeTransition(
        int transitionId,
        int? reasonId,
        List<Media> attachments,
        string comment,
        ActorType actorType,
        string actorIdentifier,
        int toActorId,
        List<Actor> actors,
        bool isExecutive = false,
        bool isContractor = false)
    {
        var now = DateTime.UtcNow;

        var transition = Process.Transitions.SingleOrDefault(p => p.Id == transitionId);
        if (transition == null)
            throw new ForbidNullTransitionException();

        if (CurrentStageId != transition.FromId)
            throw new InvalidOperationException();

        if(!transition.From.Actors.Intersect(actors).Any())
        {
            throw new UnauthorizedAccessException();
        }

        var reason = transition.ReasonList.Where(p => p.Id == reasonId).SingleOrDefault();

        //report.Priority = transitionInfo.Priority != null ? transitionInfo.Priority.Value : report.Priority;
        //Visibility = transitionInfo.Visibility ?? report.Visibility;
        CurrentStageId = transition.ToId;
        LastStatus = transition.To.Status;
        ReportState = transition.ReportState;
        LastStatusDateTime = now;
        LastTransitionId = transition.Id;
        LastReasonId = reasonId;
        LastReason = reason;


        //Check if the actor is an executive or contractor, if so, set the executor or contractor of the report
        if (actorType == ActorType.Person)
        {
            if (isExecutive)
            {
                ExecutiveId = actorIdentifier;
            }
            else if (isContractor)
            {
                ContractorId = actorIdentifier;
            }
        }

        //CurrentActor = transition.To.Actors.Where(ca => ca.Id == toActorId).SingleOrDefault();
        //TODO: Check if toActorId is allowed
        CurrentActorId = toActorId;

        //if (actorIds.Count == 0)
        //{
        //    actorIds = transition.To.Actors.Select(p => p.Id).ToList();
        //}

        //CurrentActorsStr = "";
        //actorIds.ForEach(p => CurrentActorsStr = $"{CurrentActorsStr}|{p}");
        //CurrentActorsStr = CurrentActorsStr + "|";

        var duration = (now - LastStatusDateTime).TotalSeconds;

        var log = TransitionLog.CreateTransition(Id, transitionId, comment, attachments, ReportMessages.Refered, actorType, actorIdentifier, reasonId, duration, transition.IsTransitionLogPublic);

        TransitionLogs.Add(log);

        if (ReportState == ReportState.Finished || ReportState == ReportState.Accepted)
        {
            Random random = new Random();
            Duration = (now - Sent).TotalSeconds;
            LastStatus = "پایان یافته";
            if (Feedback != null)
            {
                Feedback.Creation = now;
                Feedback.LastSent = null;
                Feedback.ReportId = Id;
                Feedback.UserId = CitizenId;
                Feedback.Token = random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString();
                Feedback.TryCount = 0;
            }
            else
            {
                Feedback = new Feedback()
                {
                    ShahrbinInstanceId = ShahrbinInstanceId,
                    Creation = now,
                    LastSent = null,
                    ReportId = Id,
                    UserId = CitizenId,
                    Token = random.Next(10000, 99999).ToString() + random.Next(10000, 99999).ToString()
                };
            }
        }

        autoTransition();
    }


    private void moveToStage(
        bool isAccepted,
        int stageId,
        int toActorId,
        string comment,
        List<Media> attachments,
        string inspectorId,
        Visibility? visibility)
    {
        var now = DateTime.UtcNow;
        var duration = now - LastStatusDateTime;

        //set the inspector id
        InspectorId = inspectorId;

        if (!isAccepted)
        {
            ReportState = ReportState.Finished;
        }
        else
        {
            var stage = Process.Stages.Where(p => p.Id == stageId).SingleOrDefault();
            if (stage is null)
                throw new NullStageException();

            //_report.Priority = transitionInfo.Priority != null ? transitionInfo.Priority.Value : _report.Priority;
            Visibility = visibility ?? Visibility;
            CurrentStageId = stage.Id;
            LastStatus = "ارجاع به " + stage.Name;
            ReportState = ReportState.Live;
            LastStatusDateTime = now;
            LastTransitionId = null;
            LastReasonId = null;
            CurrentActorId = toActorId;
        }

        TransitionLog.CreateMoveToStage(
            Id,
            comment,
            attachments,
            "",
            ActorType.Person,
            inspectorId,
            duration.TotalSeconds,
            true);

        if (isAccepted)
        {
            autoTransition();
        }

        /*
        await CommunicationServices.AddNotification(
            new Message()
            {
                ShahrbinInstanceId = _report.ShahrbinInstanceId,
                Title = "تغییر در وضعیت درخواست" + " - " + _report.TrackingNumber,
                Content = log.Message,
                DateTime = log.DateTime,
                MessageType = MessageType.Report,
                SubjectId = _report.Id,
                Recepients = new List<MessageRecepient>()
                {
                        new MessageRecepient() { Type = RecepientType.Person, ToId = _report.CitizenId }
                }
            },
            _context);

        if (moveToStageModel.IsAccepted)
        {
            await autoTransition2();
        }

        await _hub.Clients.All.Update();
        */
    }
    #endregion



    /*
     *
     var _instanceId = ShahrbinInstanceId;

        foreach (var transition in possibleTransitions)
        {
            var userActorIdentifiers = transition.To.Actors.Where(p => p.Type == ActorType.Person).Select(p => p.Identifier).ToList();
            var roleActorIdentifiers = transition.To.Actors.Where(p => p.Type == ActorType.Role).Select(p => p.Identifier).ToList();
            var userActors = _settings.InstanceSettings[_instanceId].UserActors.Data.Where(p => userActorIdentifiers.Contains(p.Id)).ToList();
            var roleActors = _settings.InstanceSettings[_instanceId].RoleActors.Data.Where(p => roleActorIdentifiers.Contains(p.Id)).ToList();

            var t = new PossibleTransitionDto()
            {
                StageTitle = transition.To.DisplayName,
                ReasonList = transition.ReasonList,
                TransitionId = transition.Id,
                CanSendMessageToCitizen = transition.CanSendMessageToCitizen
            };
            var actorList = new List<ActorDto>();
            foreach (var actor in transition.To.Actors)
            {
                //TODO: Is this true that if actor has not assigned any reagion should be included?
                if (Address.RegionId == null || actor.Regions.Count == 0 ||
                    actor.Regions.Select(p => p.Id).ToList().Contains(Address.RegionId.Value))
                {
                    actorList.Add(new ActorDto()
                    {
                        Id = actor.Id,
                        Identifier = actor.Identifier,
                        Type = actor.Type,
                        FirstName = (actor.Type == ActorType.Role) ? roleActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault() :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.FirstName).FirstOrDefault(),
                        LastName = (actor.Type == ActorType.Role) ? "" :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.LastName).FirstOrDefault(),
                        Title = (actor.Type == ActorType.Role) ? roleActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault() :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault(),
                    });
                }
            }

            //string contractorIdentifier = null;

            //Add persons in each role actor
            List<ActorDto> finalActors = new List<ActorDto>();
            for (var i = 0; i < actorList.Count; i++)
            {
                if (actorList[i].Type == ActorType.Role)
                {
                    var role = _settings.Roles.Data.Find(p => p.Id == actorList[i].Identifier);
                    if (role.Name == "Citizen")
                    {
                        finalActors.Add(actorList[i]);
                        continue;
                    }
                    if (role.Name == "Operator")
                    {
                        finalActors.Add(actorList[i]);
                        continue;
                    }

                    List<ApplicationUser> usersInRole = null;
                    if (role.Name == "Contractor")
                    {
                        ////contractorIdentifier = role.Id;
                        //usersInRole = await _context.Users.Where(p => p.Executeves.Any(q => q.Id == user.Id)).ToListAsync();
                        var executive = _settings.UsersInRoles.Data["Executive"].Where(p => p.Id == user.Id).SingleOrDefault();
                        if (executive != null)
                        {
                            usersInRole = executive.Contractors.ToList();
                        }
                        else
                        {
                            usersInRole = _settings.UsersInRoles.Data["Contractor"].ToList();
                        }
                    }
                    else
                    {
                        finalActors.Add(actorList[i]);
                        //usersInRole = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(role.Name);
                        usersInRole = _settings.UsersInRoles.Data[role.Name];
                    }

                    var actorsForUsersInRole = _settings.InstanceSettings[_instanceId].Actors.Data
                        .Where(p => usersInRole.Select(a => a.Id).ToList().Contains(p.Identifier))
                        .ToList();

                    //actorList[i].Actors = new List<ActorDto>();
                    foreach (var actor in actorsForUsersInRole)
                    {
                        finalActors.Add(new ActorDto()
                        {
                            Id = actor.Id,
                            Identifier = actor.Identifier,
                            Type = actor.Type,
                            FirstName = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.FirstName).FirstOrDefault(),
                            LastName = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.LastName).FirstOrDefault(),
                            Organization = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.Organization).FirstOrDefault(),
                            PhoneNumber = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.PhoneNumber).FirstOrDefault(),
                        });
                    }
                }
                else
                {
                    finalActors.Add(actorList[i]);
                }
            }

            ////FIX IT!
            ////Remove contractor role
            //if (contractorIdentifier != null)
            //{
            //    var con = actorList.Find(p => p.Identifier == contractorIdentifier);
            //    actorList.Remove(con);
            //}

            t.Actors = finalActors;
            result.Add(t);
        }

        return result;
     *
     * */

}

