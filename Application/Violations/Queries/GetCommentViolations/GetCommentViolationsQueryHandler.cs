using Application.Common.Interfaces.Persistence;
using Application.Violations.Queries.GetCommentViolations;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Queries.GetReportViolations;

internal class GetCommentViolationsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCommentViolationsQuery, Result<PagedList<ViolationResponse>>>
{
    public async Task<Result<PagedList<ViolationResponse>>> Handle(GetCommentViolationsQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Violation>()
            .AsNoTracking()
            .Where(v => v.CommentId == request.CommentId)
            .Select(ViolationResponse.GetSelector());

        var result = await PagedList<ViolationResponse>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);

        return result;
    }
}
