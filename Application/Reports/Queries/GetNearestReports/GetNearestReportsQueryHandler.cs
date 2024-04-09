using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetNearestReports;

internal sealed class GetNearestReportsQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetNearestReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{

    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetNearestReportsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetNearest(
            request.InstanceId,
            request.Longitude,
            request.Latitude,
            GetCitizenReportsResponse.GetSelector(request.UserId),
            request.PagingInfo);

        return result;
    }
}
