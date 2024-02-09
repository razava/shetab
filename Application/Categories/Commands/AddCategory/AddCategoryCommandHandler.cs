using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Commands.AddCategory;

internal sealed class AddCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<AddCategoryCommand, Result<Category>>
{
   
    public async Task<Result<Category>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
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
            request.FormId);

        categoryRepository.Insert(category);
        await unitOfWork.SaveAsync();

        return category;
    }
}
