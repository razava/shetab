using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetUserReports;

internal sealed class GetUserReportsQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetUserReportsQuery, Result<PagedList<GetUserReportsResponse>>>
{
    public async Task<Result<PagedList<GetUserReportsResponse>>> Handle(
        GetUserReportsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetCitizenReports(
            request.UserId,
            request.instanceId,
            GetUserReportsResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
