using Application.Comments.Queries.GetAllCommentsQuery;
using Application.Common.Interfaces.Persistence;

namespace Application.Violations.Queries.GetCommentViolationsList;

public record GetCommentViolationsListQuery(int InstanceId, PagingInfo PagingInfo)
    : IRequest<Result<PagedList<GetCommentsResponse>>>;
