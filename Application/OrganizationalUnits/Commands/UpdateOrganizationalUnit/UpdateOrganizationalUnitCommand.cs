using Domain.Models.Relational;

namespace Application.OrganizationalUnits.Commands.UpdateOrganizationalUnit;

public record UpdateOrganizationalUnitCommand(
    int OrganizationalUnitId,
    string Title,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds) : IRequest<Result<OrganizationalUnit>>;
