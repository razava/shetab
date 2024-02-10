using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands.AddCategory;

internal sealed class AddCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<AddCategoryCommand, Result<Category>>
{
   
    public async Task<Result<Category>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var roleId = await unitOfWork.DbContext.Set<Category>()
            .Where(c => c.Id == request.ParentId)
            .Select(c => c.Role.Id)
            .SingleOrDefaultAsync();
        if (roleId is null)
            return NotFoundErrors.Category;
        //TODO: perform required operations
        var category = Category.Create(
            request.InstanceId,
            request.Code,
            request.Title,
            request.Description,
            request.Order,
            request.ParentId,
            roleId,
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
