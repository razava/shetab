using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Categories.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        //TODO: perform required operations
        var category = await categoryRepository.GetSingleAsync(c => c.Id == request.Id);
        if (category is null)
            return NotFoundErrors.Category;
        
        category.Update(isDeleted: request.IsDeleted);

        categoryRepository.Update(category);

        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.CategoryStatus);
    }
}
