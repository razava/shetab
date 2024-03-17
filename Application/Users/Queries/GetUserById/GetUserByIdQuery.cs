using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using System.Linq.Expressions;

namespace Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(string UserId) : IRequest<Result<AdminGetUserDetailsResponse>>;

public record AdminGetUserDetailsResponse(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    string Organization,
    string NationalId,
    bool TwoFactorEnabled,
    Media? Avatar,
    string PhoneNumber,
    string PhoneNumber2,
    Gender Gender,
    Education Education,
    DateTime? BirthDate)
{
    public static Expression<Func<ApplicationUser, AdminGetUserDetailsResponse>> GetSelector()
    {
        Expression<Func<ApplicationUser, AdminGetUserDetailsResponse>> selector
            = user => new AdminGetUserDetailsResponse(
                user.Id,
                user.UserName ?? "",
                user.FirstName,
                user.LastName,
                user.Title,
                user.Organization,
                user.NationalId,
                user.TwoFactorEnabled,
                user.Avatar,
                user.PhoneNumber ?? "",
                user.PhoneNumber2,
                user.Gender,
                user.Education,
                user.BirthDate);
        return selector;
    }
}