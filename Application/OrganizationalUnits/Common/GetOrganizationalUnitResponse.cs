using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.OrganizationalUnits.Common;

public record GetOrganizationalUnitResponse(
    int Id,
    string Title,
    int? ActorId,
    OrganizationalUnitType? Type,
    IEnumerable<GetOrganizationalUnitResponse> OrganizationalUnits)
{
    public static Expression<Func<OrganizationalUnit, GetOrganizationalUnitResponse>> GetSelector()
    {
        Expression<Func<OrganizationalUnit, GetOrganizationalUnitResponse>> selector
            = news => new GetOrganizationalUnitResponse(
                news.Id,
                news.Title,
                news.ActorId,
                news.Type,
                news.OrganizationalUnits.Select(o =>
                    new GetOrganizationalUnitResponse(o.Id, o.Title, o.ActorId, o.Type, new List<GetOrganizationalUnitResponse>()))
                );
        return selector;
    }
}