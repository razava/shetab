using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.Reports.Queries.GetRecentReports;

internal sealed class GetRecentReportsQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetRecentReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{
    
    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetRecentReportsQuery request,
        CancellationToken cancellationToken)
    {

        Expression<Func<Report, bool>> filter = r =>
            r.ReportState != ReportState.NeedAcceptance
            && r.Visibility == Visibility.EveryOne
            && r.ShahrbinInstanceId == request.InstanceId
            && !r.IsDeleted;

        var result = await reportRepository.GetRecentReports(
            filter,
            GetCitizenReportsResponse.GetSelector(request.UserId),
            request.PagingInfo);

        return result;
    }
}
