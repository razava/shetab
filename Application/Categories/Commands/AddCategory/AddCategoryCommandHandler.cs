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
        var category = request.Category;
        _categoryRepository.Insert(category);
        await _unitOfWork.SaveAsync();
        return category;
    }
}
