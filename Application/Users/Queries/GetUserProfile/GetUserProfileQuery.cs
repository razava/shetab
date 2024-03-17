using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Queries.GetUserProfile;

public sealed record GetUserProfileQuery(
    string UserId) : IRequest<Result<ApplicationUser>>;
