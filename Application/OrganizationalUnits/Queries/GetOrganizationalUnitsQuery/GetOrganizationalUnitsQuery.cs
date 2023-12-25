using Application.Common.FilterModels;
using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitsQuery;

public record GetOrganizationalUnitsQuery(int InstanceId, QueryFilterModel? FilterModel = default!) : IRequest<List<OrganizationalUnit>>;
