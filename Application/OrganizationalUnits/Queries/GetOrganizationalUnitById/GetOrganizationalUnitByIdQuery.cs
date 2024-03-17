using Application.OrganizationalUnits.Common;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitById;

public record GetOrganizationalUnitByIdQuery(int OrganizationalUnitId) : IRequest<Result<GetOrganizationalUnitResponse>>;
