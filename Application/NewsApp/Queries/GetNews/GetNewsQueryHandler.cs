using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.NewsApp.Queries.GetNews;

internal class GetNewsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetNewsQuery, Result<PagedList<GetNewsResponse>>>
{

    public async Task<Result<PagedList<GetNewsResponse>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<News>()
            .Where(n => request.ReturnAll || n.IsDeleted == false)
            .AsNoTracking()
            .Select(GetNewsResponse.GetSelector());

        var result = await PagedList<GetNewsResponse>.ToPagedList(query, request.pagingInfo.PageNumber, request.pagingInfo.PageSize);
        return result;
    }
}
