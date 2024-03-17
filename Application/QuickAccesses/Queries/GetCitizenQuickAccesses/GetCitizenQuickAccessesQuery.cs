using Application.Common.FilterModels;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.QuickAccesses.Queries.GetCitizenQuickAccesses;

public record GetCitizenQuickAccessesQuery(
    int InstanceId,
    List<string> Roles,
    QueryFilterModel? FilterModel = default!,
    bool ReturnAll = false) : IRequest<Result<List<CitizenGetQuickAccessResponse>>>;

public record CitizenGetQuickAccessResponse(
    int Id,
    string Title,
    int Order,
    int CategoryId,
    Media Media)
{
    public static Expression<Func<QuickAccess, CitizenGetQuickAccessResponse>> GetSelector()
    {
        Expression<Func<QuickAccess, CitizenGetQuickAccessResponse>> selector
            = quick => new CitizenGetQuickAccessResponse(
                quick.Id,
                quick.Title,
                quick.Order,
                quick.CategoryId,
                quick.Media);
        return selector;
    }
}