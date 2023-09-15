using System.Linq.Expressions;

namespace Api.Services.Dal;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public Task<PagedList<TEntity>> GetPagedAsync(
        PagingInfo paging,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public TEntity? GetByID(object id);
    public Task<TEntity?> GetByIDAsync(object id);
    public void Insert(TEntity entity);
    public void Delete(object id);
    public void Delete(TEntity entityToDelete);
    public void Update(TEntity entityToUpdate);
    public int Count(Expression<Func<TEntity, bool>> filter = null);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
}
