using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetUserReports;

internal sealed class GetUserReportsQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetUserReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{
    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetUserReportsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetCitizenReports(
            request.UserId,
            GetCitizenReportsResponse.GetSelector(request.UserId),
            request.PagingInfo);

        return result;
    }
}
