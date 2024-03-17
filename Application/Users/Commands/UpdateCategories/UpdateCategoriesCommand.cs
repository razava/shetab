namespace Application.Users.Commands.UpdateCategories;

public record UpdateCategoriesCommand(string id, List<int> CategoryIds) : IRequest<Result<bool>>;
