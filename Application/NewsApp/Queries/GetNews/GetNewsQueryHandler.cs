using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using System.Linq.Expressions;

namespace Application.NewsApp.Queries.UpdateNewsCommand;

internal class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, List<News>>
{
    private readonly INewsRepository _newsRepository;

    public GetNewsQueryHandler(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<List<News>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<News, bool>>? filter = q => q.IsDeleted != false;

        
        var result = await _newsRepository.GetAsync(filter, false);

        return result.ToList();
    }
}
