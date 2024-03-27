using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand(
    string UserId,
    string? FirstName = null,
    string? LastName = null,
    string? Title = null,
    string? Organization = null,
    string? NationalId = null,
    bool? TwoFactorEnabled = null,
    Gender? Gender = null,
    Education? Education = null,
    DateTime? BirthDate = null,
    string? PhoneNumber2 = null,
    bool? SmsAlert = null
    ) : IRequest<Result<ApplicationUser>>;
