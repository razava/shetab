using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) 
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailResponse>>
{

    public async Task<Result<CategoryDetailResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform instanceId check

        var result =await categoryRepository.GetById(request.Id, "Form", false);

        if (result is null)
            return NotFoundErrors.Category;

        return CategoryDetailResponse.FromCategory(result);
    }
}
