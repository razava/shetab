using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.GetNews;

internal class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, List<News>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<List<News>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var result = await _newsRepository.GetAsync(n => (request.ReturnAll || n.IsDeleted == false), false);

        return result.ToList();
    }
}
