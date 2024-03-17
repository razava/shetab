using Application.Reports.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using System.Linq.Expressions;

namespace Application.Users.Queries.GetUserProfile;

public sealed record GetUserProfileQuery(
    string UserId) : IRequest<Result<GetProfileResponse>>;


public record GetProfileResponse(
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    string Organization,
    string PhoneNumber,
    string PhoneNumber2,
    string NationalId,
    bool TwoFactorEnabled,
    Gender Gender,
    Education Education,
    DateTime? BirthDate,
    Media? Avatar)
{
    public static Expression<Func<ApplicationUser, GetProfileResponse>> GetSelector()
    {
        Expression<Func<ApplicationUser, GetProfileResponse>> selector
            = user => new GetProfileResponse(
                user.UserName!,
                user.FirstName,
                user.LastName,
                user.Title,
                user.Organization,
                user.PhoneNumber ?? "",
                user.PhoneNumber2,
                user.NationalId,
                user.TwoFactorEnabled,
                user.Gender,
                user.Education,
                user.BirthDate,
                user.Avatar);
                
        return selector;
    }
}
