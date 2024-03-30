using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Mapster;
using SharedKernel.ExtensionMethods;

namespace Application.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler(ICategoryRepository categoryRepository) 
    : IRequestHandler<GetCategoryQuery, Result<CategoryResponse>>
{
   
    public async Task<Result<CategoryResponse>> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken)
    {

        var result = await categoryRepository.GetCategories(request.InstanceId, request.Roles, request.ReturnAll);
        if (result is null)
            return NotFoundErrors.Category;
        result.Compress();

        return result.Adapt<CategoryResponse>();
    }
}
