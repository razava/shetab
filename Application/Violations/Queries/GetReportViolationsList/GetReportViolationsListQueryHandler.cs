using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Violations.Queries.GetReportViolationsList;

internal class GetReportViolationsListQueryHandler(IViolationRepository violationRepository)
    : IRequestHandler<GetReportViolationsListQuery, Result<PagedList<GetReportsResponse>>>
{
    public async Task<Result<PagedList<GetReportsResponse>>> Handle(GetReportViolationsListQuery request, CancellationToken cancellationToken)
    {
        var result = await violationRepository.GetReportViolationList(
            request.InstanceId,
            GetReportsResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
