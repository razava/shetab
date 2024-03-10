using Application.Users.Common;

namespace Application.Users.Queries.GetUserRoles;

public record GetUserRolesQuery(string UserId) : IRequest<Result<List<IsInRoleModel>>>;
