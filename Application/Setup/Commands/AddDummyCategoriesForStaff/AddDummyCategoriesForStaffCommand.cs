namespace Application.Setup.Commands.AddDummyCategoriesForStaff;

public record AddDummyCategoriesForStaffCommand(int instanceId) : IRequest<Result<bool>>;
