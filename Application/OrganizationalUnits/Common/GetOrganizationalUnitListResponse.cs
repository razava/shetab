using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.OrganizationalUnits.Common;

public record GetOrganizationalUnitListResponse(
    int Id,
    string Title)
{
    public static Expression<Func<OrganizationalUnit, GetOrganizationalUnitListResponse>> GetSelector()
    {
        Expression<Func<OrganizationalUnit, GetOrganizationalUnitListResponse>> selector
            = news => new GetOrganizationalUnitListResponse(
                news.Id,
                news.Title);
        return selector;
    }
}
