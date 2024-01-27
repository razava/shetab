using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(string UserId) : IRequest<Result<ApplicationUser>>;