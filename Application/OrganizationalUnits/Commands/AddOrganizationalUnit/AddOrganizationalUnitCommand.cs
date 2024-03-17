using Domain.Models.Relational;

namespace Application.OrganizationalUnits.Commands.AddOrganizationalUnit;

public record AddOrganizationalUnitCommand(
    int InstanceId,
    string Title,
    string Username,
    string Password,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds) : IRequest<Result<OrganizationalUnit>>;