using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(string UserId) : IRequest<Result<ApplicationUser>>;