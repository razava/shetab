using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Commands.UpdateOrganizationalUnitCommand;

public record UpdateOrganizationalUnitCommand(
    int OrganizationalUnitId,
    string Title,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds) : IRequest<OrganizationalUnit>;