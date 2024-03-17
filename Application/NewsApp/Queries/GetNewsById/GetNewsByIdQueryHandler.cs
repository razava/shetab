using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.NewsApp.Queries.GetNewsById;

internal sealed class GetNewsByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetNewsByIdQuery, Result<GetNewsResponse>>
{

    public async Task<Result<GetNewsResponse>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<News>()
            .Where(n => n.Id == request.Id)
            .Select(GetNewsResponse.GetSelector())
            .FirstOrDefaultAsync();
        if (result is null)
            return NotFoundErrors.News;

        return result;
    }
}
