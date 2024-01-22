using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        //TODO: perform required operations
        var category = await _categoryRepository.GetSingleAsync(c => c.Id == request.Id);
        if (category is null)
            return NotFoundErrors.Category;
        
        category.Update(isDeleted: request.IsDeleted);

        _categoryRepository.Update(category);

        await _unitOfWork.SaveAsync();
        return true;
    }
}
