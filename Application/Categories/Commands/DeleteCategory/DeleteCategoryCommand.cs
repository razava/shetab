namespace Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(
    int Id,
    bool IsDeleted) : IRequest<Result<bool>>;

