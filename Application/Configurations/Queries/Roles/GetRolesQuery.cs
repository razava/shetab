using Application.Users.Common;

namespace Application.Configurations.Queries.Roles;

public record GetRolesQuery() : IRequest<Result<List<IsInRoleModel>>>;

