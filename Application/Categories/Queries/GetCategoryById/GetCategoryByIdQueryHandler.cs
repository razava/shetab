using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) 
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailResponse>>
{

    public async Task<Result<CategoryDetailResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await categoryRepository.GetByIDAsync(request.Id);

        if (result is null)
            return NotFoundErrors.Category;

        return result.Adapt<CategoryDetailResponse>();
    }
}
