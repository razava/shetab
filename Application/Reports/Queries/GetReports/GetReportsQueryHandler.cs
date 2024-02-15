using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetReportsQueryHandler(IUnitOfWork unitOfWork, IUserRepository userRepository) 
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
            if (categories.Any())
            {
                query = query.Where(r => categories.Contains(r.CategoryId));
            }
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
            else
            {
                query = query.Where(r => r.LastTransition != null && r.LastTransition.From.DisplayRoleId == request.FromRoleId);
                if (request.Roles.Contains(RoleNames.Executive) && request.FromRoleId != "RESPONSED")
                {
                    query = query.Where(r => r.Responsed == null);
                }

            }
        }
        Expression<Func<Report, bool>>? inputFilters = r =>
        ((request.FilterGetReports == null) ||
        (request.FilterGetReports.SentFromDate == null || r.Sent >= request.FilterGetReports.SentFromDate)
        && (request.FilterGetReports.SentToDate == null || r.Sent <= request.FilterGetReports.SentToDate)
        && (request.FilterGetReports.CurrentStates == null || request.FilterGetReports.CurrentStates.Contains(r.ReportState)) 
        && (request.FilterGetReports.Query == null || r.TrackingNumber.Contains(request.FilterGetReports.Query))
        && (request.FilterGetReports.PhoneNumber == null ||
        (r.Citizen.PhoneNumber!.Contains(request.FilterGetReports.PhoneNumber)) ||
        (r.Citizen.PhoneNumber2.Contains(request.FilterGetReports.PhoneNumber)) ));


        //var reports = await _reportRepository.GetPagedAsync(
        //    request.PagingInfo,
        //    filter,
        //    false,
        //    a => a.OrderBy(r => r.Sent));


        var query2 = query
            .AsNoTracking()
            .Where(inputFilters)
            .OrderBy(r => r.Sent)
            .Select(GetReportsResponse.GetSelector());
            .OrderByDescending(r => r.Priority)
            .ThenBy(r => r.Sent)
            .Select(r => GetReportsResponse.FromReport(r));

        var reports = await PagedList<GetReportsResponse>.ToPagedList(
            query2,
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return reports;
    }
}
