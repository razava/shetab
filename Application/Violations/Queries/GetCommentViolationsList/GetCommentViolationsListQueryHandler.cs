using Application.Comments.Queries.GetAllCommentsQuery;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Queries.GetCommentViolationsList;

internal class GetCommentViolationsListQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCommentViolationsListQuery, Result<PagedList<GetCommentsResponse>>>
{
    public async Task<Result<PagedList<GetCommentsResponse>>> Handle(GetCommentViolationsListQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Comment>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == request.InstanceId && r.ViolationCount > 0 && !r.IsViolationChecked)
            .Select(GetCommentsResponse.GetSelector());

        var result = await PagedList<GetCommentsResponse>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);

        return result;
    }
}
