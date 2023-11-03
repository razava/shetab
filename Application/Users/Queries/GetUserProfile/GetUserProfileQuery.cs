using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

public sealed record GetUserProfileQuery(
    string UserId) : IRequest<ApplicationUser>;
