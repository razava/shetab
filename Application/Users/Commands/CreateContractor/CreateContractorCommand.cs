using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using System.Linq.Expressions;

namespace Application.Users.Commands.CreateContractor;

public record CreateContractorCommand(
    string ExecutiveId,
    List<string> UserRoles,
    string PhoneNumber,
    string FirstName = "",
    string LastName = "",
    string Title = "",
    string Organization = ""):IRequest<Result<GetContractorsListResponse>>;

public record GetContractorsListResponse(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    Media? Avatar,
    string Organization)
{
    public static Expression<Func<ApplicationUser, GetContractorsListResponse>> GetSelector()
    {
        Expression<Func<ApplicationUser, GetContractorsListResponse>> selector
            = user => new GetContractorsListResponse(
                user.Id,
                user.UserName!,
                user.FirstName,
                user.LastName,
                user.Avatar,
                user.Organization);
        return selector;
    }
}