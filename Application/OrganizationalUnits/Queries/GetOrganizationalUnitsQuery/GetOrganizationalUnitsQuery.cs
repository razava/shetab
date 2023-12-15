using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitsQuery;

public record GetOrganizationalUnitsQuery(int InstanceId) : IRequest<List<OrganizationalUnit>>;
