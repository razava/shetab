using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;

namespace Application.NewsApp.Queries.GetNewsById;

internal sealed class GetNewsByIdQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsByIdQuery, Result<GetNewsResponse>>
{

    public async Task<Result<GetNewsResponse>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await newsRepository.GetNewsById(request.Id, GetNewsResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.News;

        return result;
    }
}
