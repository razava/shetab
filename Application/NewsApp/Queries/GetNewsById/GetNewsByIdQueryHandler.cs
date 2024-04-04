using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;

namespace Application.NewsApp.Queries.GetNewsById;

internal sealed class GetNewsByIdQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsByIdQuery, Result<GetNewsByIdResponse>>
{

    public async Task<Result<GetNewsByIdResponse>> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await newsRepository.GetNewsById(request.Id, GetNewsByIdResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.News;

        return result;
    }
}
