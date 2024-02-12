using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetCategoryById;

internal sealed class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailResponse>>
{

    public async Task<Result<CategoryDetailResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await unitOfWork.DbContext.Set<Category>()
            .Where(c => c.Id == request.Id)
            .Include(c => c.Form)
            .SingleOrDefaultAsync();

        if (result is null)
            return NotFoundErrors.Category;

        return CategoryDetailResponse.FromCategory(result);
    }
}
