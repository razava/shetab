﻿using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

internal sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        //TODO: perform required operations
        var category = await _categoryRepository.GetSingleAsync(c => c.Id == request.Id);
        if (category is null)
        {
            throw new Exception("Not found!");
        }
        category.Update(isDeleted: request.IsDeleted);

        _categoryRepository.Update(category);

        await _unitOfWork.SaveAsync();
        return true;
    }
}
