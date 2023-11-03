using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    string UserId,
    string? FirstName,
    string? LastName
    ) : IRequest<ApplicationUser>;