using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetCitizenReports;

internal sealed class GetCitizenReportsQueryHandler(IReportRepository reportRepository)
    : IRequestHandler<GetCitizenReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{
    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetCitizenReportsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetCitizenReports(
            request.UserId,
            request.instanceId,
            GetCitizenReportsResponse.GetSelector(request.UserId),
            request.PagingInfo);

        return result;
    }
}
