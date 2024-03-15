using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.GetNews;

internal class GetNewsQueryHandler(INewsRepository newsRepository) : IRequestHandler<GetNewsQuery, Result<List<News>>>
{

    public async Task<Result<List<News>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var result = await newsRepository.GetAsync(n => (request.ReturnAll || n.IsDeleted == false), false, null, "Image");

        return result.ToList();
    }
}
