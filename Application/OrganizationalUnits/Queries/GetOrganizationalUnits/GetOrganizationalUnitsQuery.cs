using Application.Common.FilterModels;
using Application.OrganizationalUnits.Common;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnits;

public record GetOrganizationalUnitsQuery(
    int InstanceId, 
    QueryFilterModel? FilterModel = default!) 
    : IRequest<Result<List<GetOrganizationalUnitListResponse>>>;
