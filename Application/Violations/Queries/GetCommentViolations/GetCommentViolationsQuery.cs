using Application.Common.Interfaces.Persistence;
using Application.Violations.Queries.GetReportViolations;

namespace Application.Violations.Queries.GetCommentViolations;

public record GetCommentViolationsQuery(Guid CommentId, PagingInfo PagingInfo)
    : IRequest<Result<PagedList<ViolationResponse>>>;
