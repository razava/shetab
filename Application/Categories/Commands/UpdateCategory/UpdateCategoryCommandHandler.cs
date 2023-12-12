using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        //TODO: perform required operations
        var category = await _categoryRepository.GetSingleAsync(c => c.Id == request.id);
        if (category is  null)
        {
            throw new Exception("Not found!");
        }
        category.Update(
            request.Code,
            request.Title,
            request.Description,
            request.Order,
            request.ParentId,
            request.Duration,
            request.ResponseDuration,
            request.ProcessId,
            request.IsDeleted,
            request.ObjectionAllowed,
            request.EdittingAllowed,
            request.HideMap,
            request.AttachmentDescription,
            request.FormElements);

        _categoryRepository.Update(category);

        await _unitOfWork.SaveAsync();
        return category;
    }
}
