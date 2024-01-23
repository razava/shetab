using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByUserIdQuery;

public record GetOrganizationalUnitByUserIdQuery(string UserId) : IRequest<Result<OrganizationalUnit>>;
