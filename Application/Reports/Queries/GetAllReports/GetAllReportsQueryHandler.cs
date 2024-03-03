using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Reports.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Reports.Queries.GetAllReports;

internal sealed class GetAllReportsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetAllReportsQuery, Result<PagedList<GetReportsResponse>>>
{
    
    public async Task<Result<PagedList<GetReportsResponse>>> Handle(
        GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;

        var query = context.Set<Report>().Where(t => true);


        if (request.Roles.Contains(RoleNames.Mayor))
        {

        }
        else if (request.Roles.Contains(RoleNames.Operator))
        {
            
        }
        else if (request.Roles.Contains(RoleNames.Manager))
        {

        }
        else if (request.Roles.Contains(RoleNames.Inspector))
        {

        }
        else
        {
            query = query.Where(r => r.TransitionLogs.Any(rt => rt.ActorIdentifier == request.UserId));
        }

        
        Expression<Func<Report, bool>>? inputFilters = r =>
        ((request.FilterGetReports == null) ||
        (request.FilterGetReports.SentFromDate == null || r.Sent >= request.FilterGetReports.SentFromDate)
        && (request.FilterGetReports.SentToDate == null || r.Sent <= request.FilterGetReports.SentToDate)
        && (request.FilterGetReports.CurrentStates == null || request.FilterGetReports.CurrentStates.Contains(r.ReportState))
        && (request.FilterGetReports.Query == null || r.TrackingNumber.Contains(request.FilterGetReports.Query))
        && (request.FilterGetReports.PhoneNumber == null ||
        (r.Citizen.PhoneNumber!.Contains(request.FilterGetReports.PhoneNumber)) ||
        (r.Citizen.PhoneNumber2.Contains(request.FilterGetReports.PhoneNumber))));

        var query2 = query
            .AsNoTracking()
            .Where(inputFilters)
            .OrderBy(r => r.Sent)
            .Select(GetReportsResponse.GetSelector());

        var reports = await PagedList<GetReportsResponse>.ToPagedList(
            query2,
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return reports;
    }
}
