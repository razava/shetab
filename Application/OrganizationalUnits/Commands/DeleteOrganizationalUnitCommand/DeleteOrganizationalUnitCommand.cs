using MediatR;

namespace Application.OrganizationalUnits.Commands.DeleteOrganizationalUnitCommand;

public record DeleteOrganizationalUnitCommand(int Id) : IRequest<bool>;
