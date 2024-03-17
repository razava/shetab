using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetComments;

internal sealed class GetCommentsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetCommentsQuery, Result<PagedList<GetReportCommentsResponse>>>
{
    public async Task<Result<PagedList<GetReportCommentsResponse>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var query = context.Set<Comment>()
            .AsNoTracking()
            .Where(c => c.ReportId == request.ReportId).Include(e => e.User)
            .Select(GetReportCommentsResponse.GetSelector(request.UserId));
        var result = await PagedList<GetReportCommentsResponse>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);

        return result;
    }
}