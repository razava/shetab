using Application.Users.Common;

namespace Application.Users.Queries.GetUserRoles;

public record GetRolesQuery() : IRequest<Result<List<RoleResponse>>>;


