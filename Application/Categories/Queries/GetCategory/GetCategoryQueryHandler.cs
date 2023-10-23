using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, List<Category>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await _categoryRepository.GetAsync(p => p.ShahrbinInstanceId == request.InstanceId);

        return result.ToList();
    }
}
