using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetReportsQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetReportsQuery, Result<PagedList<GetReportsResponse>>>
{
    
    public async Task<Result<PagedList<GetReportsResponse>>> Handle(
        GetReportsQuery request,
        CancellationToken cancellationToken)
    {
        
        var result = await reportRepository.GetReports(
            request.InstanceId,
            request.UserId,
            request.Roles,
            request.FromRoleId,
            GetReportsResponse.GetSelector(),
            request.ReportFilters,
            request.PagingInfo);

        return result;
    }
}
