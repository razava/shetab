using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetComments;

internal sealed class GetCommentsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCommentsQuery, Result<PagedList<Comment>>>
{
    public async Task<Result<PagedList<Comment>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var query = context.Set<Comment>().Where(c => c.ReportId == request.ReportId).Include(e => e.User);
        var result = await PagedList<Comment>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);
        return result;
    }
}