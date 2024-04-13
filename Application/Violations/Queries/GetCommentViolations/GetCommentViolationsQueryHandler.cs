using Application.Common.Interfaces.Persistence;
using Application.Violations.Queries.GetCommentViolations;

namespace Application.Violations.Queries.GetReportViolations;

internal class GetCommentViolationsQueryHandler(IViolationRepository violationRepository)
    : IRequestHandler<GetCommentViolationsQuery, Result<PagedList<ViolationResponse>>>
{
    public async Task<Result<PagedList<ViolationResponse>>> Handle(GetCommentViolationsQuery request, CancellationToken cancellationToken)
    {
        var result = await violationRepository.GetCommentViolations(
            request.CommentId,
            ViolationResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
