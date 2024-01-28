using Application.Users.Common;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Configurations.Queries.Roles;

public record GetRolesQuery() : IRequest<Result<List<IsInRoleModel>>>;

