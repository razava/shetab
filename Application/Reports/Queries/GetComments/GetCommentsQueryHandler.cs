using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetComments;

internal sealed class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, PagedList<Comment>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCommentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<Comment>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var context = _unitOfWork.DbContext;
        var query = context.Set<Comment>().Where(c => c.ReportId == request.ReportId).Include(e => e.User);
        var result = await PagedList<Comment>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);
        return result;
    }
}