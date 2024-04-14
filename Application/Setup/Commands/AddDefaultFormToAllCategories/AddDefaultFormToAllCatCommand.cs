namespace Application.Setup.Commands.AddDefaultFormToAllCategories;

public record AddDefaultFormToAllCatCommand(int instanceId) : IRequest<Result<bool>>;
