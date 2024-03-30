using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class NewsRepository : GenericRepository<News>, INewsRepository
{
    private readonly ApplicationDbContext _dbContext;
    public NewsRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<T>> GetNews<T>(PagingInfo pagingInfo, Expression<Func<News, T>> selector, bool returnAll = false)
    {
        var query = _dbContext.News.AsNoTracking()
            .Where(n => returnAll || n.IsDeleted == false)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);

    }

    public async Task<T?> GetNewsById<T>(int id, Expression<Func<News, T>> selector)
    {
        var result = await _dbContext.News.AsNoTracking()
            .Where(n => n.Id == id)
            .Select(selector)
            .SingleOrDefaultAsync();
        return result;
    }
}
