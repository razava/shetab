using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using System.Linq.Expressions;

namespace Application.Users.Queries.GetUsers;

public record GetUsersQuery(PagingInfo PagingInfo, int InstanceId, UserFilters UserFilters) 
    : IRequest<Result<PagedList<GetUsersListResponse>>>;

public record UserFilters(
    string? Query,
    List<int>? Regions,
    List<string>? Roles);

public record GetUsersListResponse(
    string Id,
    string? UserName,
    string FirstName,
    string LastName,
    string Title,
    Media? Avatar,
    string? PhoneNumber)
{
    public static Expression<Func<ApplicationUser, GetUsersListResponse>> GetSelector()
    {
        Expression<Func<ApplicationUser, GetUsersListResponse>> selector
            = report => new GetUsersListResponse(
                report.Id,
                report.UserName,
                report.FirstName,
                report.LastName,
                report.Title,
                report.Avatar,
                report.PhoneNumber);
        return selector;
    }
}

