using Application.OrganizationalUnits.Common;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByUserId;

public record GetOrganizationalUnitByUserIdQuery(string UserId) : IRequest<Result<GetOrganizationalUnitResponse>>;
