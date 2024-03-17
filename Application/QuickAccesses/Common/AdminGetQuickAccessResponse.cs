using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.QuickAccesses.Common;

public record AdminGetQuickAccessResponse(
    int Id,
    string Title,
    int Order,
    int CategoryId,
    Media Media,
    bool IsDeleted)
{
    public static Expression<Func<QuickAccess, AdminGetQuickAccessResponse>> GetSelector()
    {
        Expression<Func<QuickAccess, AdminGetQuickAccessResponse>> selector
            = quickAccess => new AdminGetQuickAccessResponse(
                quickAccess.Id,
                quickAccess.Title,
                quickAccess.Order,
                quickAccess.CategoryId,
                quickAccess.Media,
                quickAccess.IsDeleted);
        return selector;
    }
};