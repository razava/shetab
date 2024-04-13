using Application.Common.Interfaces.Persistence;

namespace Application.Violations.Queries.GetReportViolations;

internal class GetReportViolationsQueryHandler(IViolationRepository violationRepository)
    : IRequestHandler<GetReportViolationsQuery, Result<PagedList<ViolationResponse>>>
{
    public async Task<Result<PagedList<ViolationResponse>>> Handle(GetReportViolationsQuery request, CancellationToken cancellationToken)
    {
        var result = await violationRepository.GetReportViolations(
            request.ReportId,
            ViolationResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
