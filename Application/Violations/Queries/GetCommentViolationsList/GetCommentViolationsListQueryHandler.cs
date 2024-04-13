using Application.Comments.Queries.GetAllCommentsQuery;
using Application.Common.Interfaces.Persistence;

namespace Application.Violations.Queries.GetCommentViolationsList;

internal class GetCommentViolationsListQueryHandler(IViolationRepository violationRepository)
    : IRequestHandler<GetCommentViolationsListQuery, Result<PagedList<GetCommentsResponse>>>
{
    public async Task<Result<PagedList<GetCommentsResponse>>> Handle(GetCommentViolationsListQuery request, CancellationToken cancellationToken)
    {
        var result = await violationRepository.GetCommentViolationList(
            request.InstanceId,
            GetCommentsResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
