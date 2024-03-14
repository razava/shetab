using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetReportsQueryHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IInfoService infoService) 
    : IRequestHandler<GetReportsQuery, Result<PagedList<GetReportsResponse>>>
{
    
    public async Task<Result<PagedList<GetReportsResponse>>> Handle(
        GetReportsQuery request,
        CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;

        var actors = await userRepository.GetActorsAsync(request.UserId);
        var actorIds = actors.Select(a => a.Id).ToList();
        
        var query = context.Set<Report>().Where(t => true);


        if (request.Roles.Contains(RoleNames.Operator))
        {
            var categories = await userRepository.GetUserCategoriesAsync(request.UserId);
            query = query.Where(r => categories.Contains(r.CategoryId));
            query = query.Where(r => r.ShahrbinInstanceId == request.InstanceId);
        }

        if (request.Roles.Contains(RoleNames.Operator) && request.FromRoleId == "NEW")
        {
            query = query.Where(r => r.ReportState == ReportState.NeedAcceptance);
        }
        else
        {
            query = query.Where(r => r.CurrentActorId != null &&
                                     actorIds.Contains(r.CurrentActorId.Value));
            if (request.Roles.Contains(RoleNames.Executive) && request.FromRoleId == "RESPONSED")
            {
                query = query.Where(r => r.Responsed != null);
            }
            else if (request.Roles.Contains(RoleNames.Inspector) && request.FromRoleId == "INSPECTOR")
            {
                query = query.Where(r => r.LastTransitionId == null);
            }
            else
            {
                query = query.Where(r => r.LastTransition != null && r.LastTransition.From.DisplayRoleId == request.FromRoleId);
                if (request.Roles.Contains(RoleNames.Executive))
                {
                    query = query.Where(r => r.Responsed == null);
                }

            }
        }

        query = infoService.AddFilters(query, request.ReportFilters);



        //var reports = await _reportRepository.GetPagedAsync(
        //    request.PagingInfo,
        //    filter,
        //    false,
        //    a => a.OrderBy(r => r.Sent));


        var query2 = query
            .AsNoTracking()
            .OrderByDescending(r => r.Priority)
            .ThenBy(r => r.Sent)
            .Select(GetReportsResponse.GetSelector());

        var reports = await PagedList<GetReportsResponse>.ToPagedList(
            query2,
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return reports;
    }
}
