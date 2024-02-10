using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : IRequestHandler<UpdateCategoryCommand, Result<Category>>
{
    public async Task<Result<Category>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        //TODO: perform required operations
        var category = await categoryRepository.GetSingleAsync(c => c.Id == request.Id);
        if (category is null)
            return NotFoundErrors.Category;
        string? roleId = null;
        if(request.ParentId is not null)
        {
            roleId = await unitOfWork.DbContext.Set<Category>()
                .Where(c => c.Id == request.ParentId)
                .Select(c => c.RoleId)
                .SingleOrDefaultAsync();
            if (roleId is null)
                return NotFoundErrors.Category;
        }

        category.Update(
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

        categoryRepository.Update(category);

        await unitOfWork.SaveAsync();
        return category;
    }
}
