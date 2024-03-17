namespace Application.OrganizationalUnits.Commands.DeleteOrganizationalUnit;

public record DeleteOrganizationalUnitCommand(int Id) : IRequest<Result<bool>>;
