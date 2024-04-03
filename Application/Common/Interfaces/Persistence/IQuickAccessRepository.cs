using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IQuickAccessRepository : IGenericRepository<QuickAccess>
{
    public Task<List<T>> GetAll<T>(
        int instanceId,
        Expression<Func<QuickAccess, T>> selector,
        Expression<Func<QuickAccess, bool>> filter,
        bool returnAll = false);

    public Task<T?> GetById<T>(int id, Expression<Func<QuickAccess, T>> selector);
}
