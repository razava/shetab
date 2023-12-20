using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsById;

internal sealed class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, News>
{
    private readonly INewsRepository _newsRepository;
    public GetNewsByIdQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }
    public async Task<News> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _newsRepository.GetSingleAsync(n => n.Id == request.Id, false);
        if (result is null)
            throw new NotFoundException("News");

        return result;
    }
}
