using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ReportAggregate;
using Domain.Primitives;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface ICommentRepository : IGenericRepository<Comment>
{
    public Task<PagedList<T>> GetAll<T>(
        Expression<Func<Comment, bool>>? filter, PagingInfo pagingInfo, Expression<Func<Comment, T>> selector);
}
