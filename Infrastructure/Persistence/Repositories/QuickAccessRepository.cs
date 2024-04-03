using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class QuickAccessRepository : GenericRepository<QuickAccess>, IQuickAccessRepository
{
    private readonly ApplicationDbContext _dbContect;
    public QuickAccessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContect = dbContext;
    }

    public async Task<List<T>> GetAll<T>(
        int instanceId,
        Expression<Func<QuickAccess, T>> selector,
        Expression<Func<QuickAccess, bool>> filter,
        bool returnAll = false)
    {
        var result = await _dbContect.Set<QuickAccess>()
            .AsNoTracking()
            .Where(q => q.ShahrbinInstanceId == instanceId)
            .Where(q => returnAll || q.IsDeleted == false)
            .Where(filter)
            .OrderBy(q => q.Order)
            .Select(selector)
            .ToListAsync();
        return result;
    }

    public async Task<T?> GetById<T>(int id, Expression<Func<QuickAccess, T>> selector)
    {
        var result = await _dbContect.Set<QuickAccess>()
            .AsNoTracking()
            .Where(q => q.Id == id)
            .Select(selector)
            .SingleOrDefaultAsync();

        return result;
    }
}
