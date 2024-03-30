using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Mapster;
using SharedKernel.ExtensionMethods;

namespace Application.Categories.Queries.GetStaffCategories;

internal class GetStaffCategoriesQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<GetStaffCategoriesQuery, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(GetStaffCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetStaffCategories(request.InstanceId, request.UserId, request.Roles);
        if (result is null)
            return NotFoundErrors.Category;

        result.Compress();

        return result.Adapt<CategoryResponse>();

    }
}
