using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Queries.GetInfo;
using Application.Reports.Common;
using DocumentFormat.OpenXml.ExtendedProperties;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using Infrastructure.Info;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProcessRepository _processRepository;
    private readonly IUserRepository _userRepository;
    private readonly IInfoService _infoService; 

    public ReportRepository(
        ApplicationDbContext dbContext,
        ICategoryRepository categoryRepository,
        IProcessRepository processRepository,
        IUserRepository userRepository,
        IInfoService infoService) : base(dbContext)
    {
        _categoryRepository = categoryRepository;
        _processRepository = processRepository;
        _context = dbContext;
        _userRepository = userRepository;
        _infoService = infoService;
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



    public async Task<PagedList<T>> GetCitizenReports<T>(string userId, Expression<Func<Report, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Set<Report>()
            .AsNoTracking()
            .Where(r => r.CitizenId == userId)
            .OrderByDescending(r => r.LastStatusDateTime)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }


    public async Task<PagedList<T>> GetRecentReports<T>(Expression<Func<Report, bool>> filter, Expression<Func<Report, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Set<Report>()
            .AsNoTracking()
            .Where(filter)
            .OrderByDescending(r => r.LastStatusDateTime)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
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



}
