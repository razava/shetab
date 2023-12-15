using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Commands.AddOrganizationalUnitCommand;

public record AddOrganizationalUnitCommand(
    int InstanceId,
    string Title,
    string Username,
    string Password,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds) : IRequest<OrganizationalUnit>;