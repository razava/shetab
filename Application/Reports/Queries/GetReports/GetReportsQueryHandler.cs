using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetReportsQueryHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, IUserRepository userRepository) : IRequestHandler<GetReportsQuery, Result<PagedList<Report>>>
{
    
    public async Task<Result<PagedList<Report>>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;

        var actors = await userRepository.GetActorsAsync(request.UserId);
        var actorIds = actors.Select(a => a.Id).ToList();
        var categories = await userRepository.GetUserCategoriesAsync(request.UserId);

        System.Linq.Expressions.Expression<Func<Report, bool>>? filter;
        if (request.FromRoleId is null && request.Roles.Contains(RoleNames.Operator))
        {
            filter = r => r.ReportState == ReportState.NeedAcceptance && r.ShahrbinInstanceId == request.InstanceId
            && (!categories.Any() || categories.Contains(r.CategoryId));
        }
        else
        {
            filter = r => r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value) &&
                     r.LastTransition != null && r.LastTransition.From.DisplayRoleId == request.FromRoleId &&
                     r.ShahrbinInstanceId == request.InstanceId
                     && (!categories.Any() || categories.Contains(r.CategoryId));
        }

        System.Linq.Expressions.Expression<Func<Report, bool>>? inputFilters = r =>
        ((request.FilterGetReports == null) ||
        (request.FilterGetReports.SentFromDate == null || r.Sent >= request.FilterGetReports.SentFromDate)
        && (request.FilterGetReports.SentToDate == null || r.Sent <= request.FilterGetReports.SentToDate)
        && (request.FilterGetReports.CurrentStates == null || request.FilterGetReports.CurrentStates.Contains(r.ReportState)) 
        && (request.FilterGetReports.Query == null || r.TrackingNumber.Contains(request.FilterGetReports.Query))
        && (request.FilterGetReports.PhoneNumber == null ||
        (r.Citizen.PhoneNumber.Contains(request.FilterGetReports.PhoneNumber)) ||
        (r.Citizen.PhoneNumber2.Contains(request.FilterGetReports.PhoneNumber)) ));


        //var reports = await _reportRepository.GetPagedAsync(
        //    request.PagingInfo,
        //    filter,
        //    false,
        //    a => a.OrderBy(r => r.Sent));

        
        var query = context.Set<Report>()
            .AsNoTracking()
            .Where(filter)
            .Where(inputFilters);

        var reports = await PagedList<Report>.ToPagedList(
            query.OrderBy(r => r.Sent),
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return reports;
    }
}
