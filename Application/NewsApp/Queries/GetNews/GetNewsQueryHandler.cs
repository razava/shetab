using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.NewsApp.Queries.GetNews;

internal class GetNewsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetNewsQuery, Result<List<GetNewsResponse>>>
{

    public async Task<Result<List<GetNewsResponse>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<News>()
            .Where(n => request.ReturnAll || n.IsDeleted == false)
            .Select(GetNewsResponse.GetSelector())
            .ToListAsync();

        return result.ToList();
    }
}
