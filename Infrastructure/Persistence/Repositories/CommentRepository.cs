using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    private readonly ApplicationDbContext _dbContext;
    public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<PagedList<T>> GetAll<T>(Expression<Func<Comment, bool>>? filter, PagingInfo pagingInfo, Expression<Func<Comment, T>> selector)
    {
        var query = _dbContext.Comment.AsNoTracking();

        if (filter != null) 
        {
            query = query.Where(filter);
        }

        query = query.OrderBy(c => c.DateTime);

        var query2 = query.Select(selector);

        return await PagedList<T>.ToPagedList(query2, pagingInfo.PageNumber, pagingInfo.PageSize);

    }
}
