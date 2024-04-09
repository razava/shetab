using Application.Common.Exceptions;
using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Queries.GetInfo;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ReportAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProcessRepository _processRepository;
    private readonly IUserRepository _userRepository;
    private readonly IInfoService _infoService;
    private readonly IActorRepository _actorRepository;

    public ReportRepository(
        ApplicationDbContext dbContext,
        ICategoryRepository categoryRepository,
        IProcessRepository processRepository,
        IUserRepository userRepository,
        IInfoService infoService,
        IActorRepository actorRepository) : base(dbContext)
    {
        _categoryRepository = categoryRepository;
        _processRepository = processRepository;
        _context = dbContext;
        _userRepository = userRepository;
        _infoService = infoService;
        _actorRepository = actorRepository;
    }

    public async Task<Report?> GetByIDAsync(Guid id, bool trackChanges = true)
    {
        //TransitionLogs won't be modified, so there is no need to include them
        var result = await base.GetSingleAsync(r => r.Id == id, trackChanges, @"CurrentActor,Feedback");

        if (result is null)
            return null;

        result.Category = (await _categoryRepository.GetByIDAsync(result.CategoryId))!;
        result.Process = (await _processRepository.GetByIDAsync(result.ProcessId))!;

        return result;
    }

    public async Task<T?> GetByIdSelective<T>(Guid id, Expression<Func<Report, bool>> filter, Expression<Func<Report, T>> selector)
    {
        var result = await _context.Set<Report>()
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Where(filter)
            .Select(selector)
            .SingleOrDefaultAsync();
        return result;
    }



    public async Task<PagedList<T>> GetCitizenReports<T>(string userId, int? instanceId, Expression<Func<Report, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Set<Report>()
            .AsNoTracking()
            .Where(r => r.CitizenId == userId && (instanceId == null || r.ShahrbinInstanceId == instanceId))
            .OrderByDescending(r => r.LastStatusDateTime)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<PagedList<T>> GetNearest<T>(int instanceId, double longitude, double latitude, Expression<Func<Report, T>> selector, PagingInfo pagingInfo)
    {
        var currentLocation = new Point(longitude, latitude) { SRID = 4326 };

        var query = _context.Set<Report>()
            .Where(r => r.ReportState != ReportState.NeedAcceptance
                               && r.Visibility == Visibility.EveryOne
                               && r.ShahrbinInstanceId == instanceId
                               && r.Address.Location != null
                               && !r.IsDeleted);
        var query2 = query
            .OrderBy(r => r.Address.Location!.Distance(currentLocation))
            .AsNoTracking()
            .Select(selector);

        var reports = await PagedList<T>.ToPagedList(
           query2,
           pagingInfo.PageNumber,
           pagingInfo.PageSize);

        return reports;
    }

    
    public async Task<PagedList<T>> GetRecentReports<T>(List<string> roles, Expression<Func<Report, bool>> filter, Expression<Func<Report, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Set<Report>()
            .AsNoTracking()
            .Where(filter)
            .Where(r => (r.Category.Role.Name != null) && roles.Contains(r.Category.Role.Name))
            .OrderByDescending(r => r.LastStatusDateTime)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<PagedList<T>> GetReportComments<T>(Guid reportId, Expression<Func<Comment, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Set<Comment>()
            .AsNoTracking()
            .Where(c => c.ReportId == reportId && !c.IsReply).Include(e => e.User)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<List<HistoryResponse>> GetReportHistory(Guid id)
    {
        var result = await _context.Set<TransitionLog>()
            .Where(tl => tl.ReportId == id)
            .Include(tl => tl.Reason)
            .OrderByDescending(tl => tl.DateTime)
            .ToListAsync();

        var response = new List<HistoryResponse>();
        foreach (var transitionLog in result)
        {
            var historyResponse = transitionLog.Adapt<HistoryResponse>();
            response.Add(historyResponse);

            var actor = await _actorRepository.GetSingleAsync(a => a.Identifier == transitionLog.ActorIdentifier, false);
            if (actor == null)
                continue; //todo: handle this situation

            var actorResponse = new ActorResponse();
            if (actor.Type == ActorType.Person)
            {
                var actorUser = await _userRepository.GetSingleAsync(u => u.Id == actor.Identifier, false);
                if (actorUser == null) 
                    throw new ServerNotFoundException("خطایی رخ داد.", new ActorNotFoundException());
                actorResponse.FirstName = actorUser.FirstName;
                actorResponse.LastName = actorUser.LastName;
                actorResponse.Title = actorUser.Title;
                historyResponse.Actor = actorResponse;
            }
            if (actor.Type == ActorType.Role)
            {
                var acorRole = (await _userRepository.GetRoleActors(new List<string> { actor.Identifier }))
                    .FirstOrDefault();
                if (acorRole == null)
                    throw new ServerNotFoundException("خطایی رخ داد.", new ActorNotFoundException());
                actorResponse.Title = acorRole.Title;
                historyResponse.Actor = actorResponse;
            }
            if (actor.Type == ActorType.Auto)
            {
                actorResponse.Title = "ارجاع خودکار";
                historyResponse.Actor = actorResponse;
            }
        }

        return response;
    }

    public async Task<PagedList<T>> GetReports<T>(
        int instanceId,
        string userId,
        List<string> roles,
        string? fromRoleId,
        Expression<Func<Report, T>> selector,
        ReportFilters reportFilters,
        PagingInfo pagingInfo)
    {

        var actors = await _userRepository.GetActorsAsync(userId);
        var actorIds = actors.Select(a => a.Id).ToList();

        var query = context.Set<Report>().Where(t => true);


        if (roles.Contains(RoleNames.Operator))
        {
            var categories = await _userRepository.GetUserCategoriesAsync(userId);
            query = query.Where(r => categories.Contains(r.CategoryId));
            query = query.Where(r => r.ShahrbinInstanceId == instanceId);
        }

        if (roles.Contains(RoleNames.Operator) && fromRoleId == "NEW")
        {
            query = query.Where(r => r.ReportState == ReportState.NeedAcceptance);
        }
        else
        {
            query = query.Where(r => r.CurrentActorId != null &&
                                     actorIds.Contains(r.CurrentActorId.Value));
            if (roles.Contains(RoleNames.Executive) && fromRoleId == "RESPONSED")
            {
                query = query.Where(r => r.LastOperation == ReportOperationType.MessageToCitizen);
            }
            else if (roles.Contains(RoleNames.Inspector) && fromRoleId == "INSPECTOR")
            {
                query = query.Where(r => r.LastTransitionId == null);
            }
            else
            {
                query = query.Where(r => r.LastTransition != null && r.LastTransition.From.DisplayRoleId == fromRoleId);
                if (roles.Contains(RoleNames.Executive))
                {
                    query = query.Where(r => r.Responsed == null || r.LastOperation != ReportOperationType.MessageToCitizen);
                }

            }
        }

        query = _infoService.AddFilters(query, reportFilters);

        var query2 = query
            .AsNoTracking()
            .OrderByDescending(r => r.Priority)
            .ThenBy(r => r.Sent)
            .Select(selector);

        var result = await PagedList<T>.ToPagedList(
            query2,
            pagingInfo.PageNumber,
            pagingInfo.PageSize);

        return result;

    }


    public async Task<List<PossibleTransitionResponse>> GetPossibleTransition(Guid reportId, string userId)
    {

        var report = await GetByIDAsync(reportId);
        if (report == null)
            throw new ServerNotFoundException("خطایی رخ داد.", new ReportNotFoundException());

        //if (report.ShahrbinInstanceId != request.instanceId)
        //    return AccessDeniedErrors.General;

        var possibleTransitions = report.GetPossibleTransitions();

        var result = new List<PossibleTransitionResponse>();
        var regionId = report.Address.RegionId;

        foreach (var transition in possibleTransitions)
        {
            var userActorIdentifiers = transition.To.Actors.Where(p => p.Type == ActorType.Person).Select(p => p.Identifier).ToList();
            var roleActorIdentifiers = transition.To.Actors.Where(p => p.Type == ActorType.Role).Select(p => p.Identifier).ToList();
            var userActors = await _userRepository.GetUserActors(userActorIdentifiers);
            var roleActors = await _userRepository.GetRoleActors(roleActorIdentifiers);

            var t = new PossibleTransitionResponse()
            {
                StageTitle = transition.To.DisplayName,
                ReasonList = transition.ReasonList.ToList().Adapt<List<ProcessReasonResponse>>(),
                TransitionId = transition.Id,
                CanSendMessageToCitizen = transition.CanSendMessageToCitizen
            };
            var actorList = new List<ActorWithName>();
            foreach (var actor in transition.To.Actors)
            {
                if (actor.Type == ActorType.Auto)
                    continue;
                //TODO: Is this true that if actor has not assigned any reagion should be included?
                if (regionId == null || actor.Regions.Count == 0 ||
                    actor.Regions.Select(p => p.Id).ToList().Contains(regionId.Value))
                {
                    actorList.Add(new ActorWithName()
                    {
                        Id = actor.Id,
                        Identifier = actor.Identifier,
                        Type = actor.Type,
                        FirstName = (actor.Type == ActorType.Role) ? roleActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault()! :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.FirstName).FirstOrDefault()!,
                        LastName = (actor.Type == ActorType.Role) ? "" :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.LastName).FirstOrDefault()!,
                        Title = (actor.Type == ActorType.Role) ? roleActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault()! :
                            userActors.Where(p => p.Id == actor.Identifier).Select(p => p.Title).FirstOrDefault()!,
                    });
                }
            }

            //string contractorIdentifier = null;

            //Add persons in each role actor
            List<ActorWithName> finalActors = new List<ActorWithName>();
            for (var i = 0; i < actorList.Count; i++)
            {
                if (actorList[i].Type == ActorType.Role)
                {
                    var role = (await _userRepository.GetRoles()).Find(p => p.Id == actorList[i].Identifier);
                    if (role is null)
                        throw new ServerNotFoundException("خطایی رخ داد.", new NullActorRolesException());

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

                    List<ApplicationUser> usersInRole = new();
                    if (role.Name == "Contractor")
                    {
                        ////contractorIdentifier = role.Id;
                        //usersInRole = await _context.Users.Where(p => p.Executeves.Any(q => q.Id == user.Id)).ToListAsync();
                        //var executive = (await userRepository.GetUsersInRole("Executive")).Where(p => p.Id == userId).SingleOrDefault();
                        var executive = await _userRepository.GetSingleAsync(u => u.Id == userId, false, "Contractors");
                        if (executive != null)
                        {
                            usersInRole.AddRange(executive.Contractors.ToList());
                        }
                        else
                        {
                            usersInRole.AddRange((await _userRepository.GetUsersInRole("Contractor")).ToList());
                        }
                    }
                    else
                    {
                        finalActors.Add(actorList[i]);
                        //usersInRole = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(role.Name);
                        usersInRole = await _userRepository.GetUsersInRole(role.Name!);
                    }

                    var actorsForUsersInRole = (await _userRepository.GetActors())
                        .Where(p => usersInRole.Select(a => a.Id).ToList().Contains(p.Identifier))
                        .ToList();

                    //actorList[i].Actors = new List<ActorDto>();
                    foreach (var actor in actorsForUsersInRole)
                    {
                        finalActors.Add(new ActorWithName()
                        {
                            Id = actor.Id,
                            Identifier = actor.Identifier,
                            Type = actor.Type,
                            FirstName = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.FirstName).FirstOrDefault()!,
                            LastName = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.LastName).FirstOrDefault()!,
                            Organization = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.Organization).FirstOrDefault()!,
                            PhoneNumber = usersInRole.Where(p => p.Id == actor.Identifier).Select(p => p.PhoneNumber).FirstOrDefault()!,
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
    }

}
