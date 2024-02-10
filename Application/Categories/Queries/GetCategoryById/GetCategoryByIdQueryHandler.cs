using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
{

    public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await categoryRepository.GetByIDAsync(request.Id);

        if (result is null)
            return NotFoundErrors.Category;

        return result;
    }
}
