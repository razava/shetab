using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByIdQuery;

public record GetOrganizationalUnitByIdQuery(int OrganizationalUnitId) : IRequest<OrganizationalUnit>;
