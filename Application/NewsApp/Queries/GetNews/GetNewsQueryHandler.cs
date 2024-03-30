using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;

namespace Application.NewsApp.Queries.GetNews;

internal class GetNewsQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsQuery, Result<PagedList<GetNewsResponse>>>
{

    public async Task<Result<PagedList<GetNewsResponse>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var result = await newsRepository.GetNews(request.pagingInfo, GetNewsResponse.GetSelector(), request.ReturnAll);
        return result;
    }
}
