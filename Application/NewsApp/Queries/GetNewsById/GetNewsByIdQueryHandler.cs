using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsById;

internal sealed class GetNewsByIdQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsByIdQuery, Result<News>>
{

    public async Task<Result<News>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await newsRepository.GetSingleAsync(n => n.Id == request.Id, false);
        if (result is null)
            return NotFoundErrors.News;

        return result;
    }
}
