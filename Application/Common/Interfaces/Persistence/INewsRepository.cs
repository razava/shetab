using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface INewsRepository : IGenericRepository<News>
{
    public Task<PagedList<T>> GetNews<T>(PagingInfo pagingInfo, Expression<Func<News, T>> selector, bool returnAll = false);
    public Task<T?> GetNewsById<T>(int id, Expression<Func<News, T>> selector);
}
