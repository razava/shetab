using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;

namespace Application.Reports.Queries.GetRecentReports;

internal sealed class GetRecentReportsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetRecentReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{
    
    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetRecentReportsQuery request,
        CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext.Set<Report>();
        var query = context.Where(r => r.ReportState != ReportState.NeedAcceptance
                                       && r.Visibility == Visibility.EveryOne
                                       && r.ShahrbinInstanceId == request.InstanceId);
        var query2 = query
            .OrderByDescending(r => r.LastStatusDateTime)
            .Select(r => GetCitizenReportsResponse.FromReport(r, request.UserId));

        var reports = await PagedList<GetCitizenReportsResponse>.ToPagedList(
           query2,
           request.PagingInfo.PageNumber,
           request.PagingInfo.PageSize);

        return reports;
    }
}
