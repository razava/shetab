﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Successes;
using Application.Common.Helper;

namespace Application.Categories.Commands.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository) 
    : IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
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
            request.FormId,
            request.DefaultPriority);

        categoryRepository.Update(category);

        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.Category);
    }
}
