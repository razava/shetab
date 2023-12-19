using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Commands.AddCategory;

internal sealed class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Category>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public AddCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        //TODO: perform required operations
        var category = Category.Create(
            request.InstanceId,
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

        _categoryRepository.Insert(category);
        await _unitOfWork.SaveAsync();

        return category;
    }
}
