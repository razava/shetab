using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class FaqRepository : GenericRepository<Faq>, IFaqRepository
{
    private readonly ApplicationDbContext _dbContext;
    public FaqRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<List<T>> GetFaqs<T>(Expression<Func<Faq, bool>>? filter, Expression<Func<Faq, T>> selector)
    {
        var query = _dbContext.Faq.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);

        var result = query.Select(selector).ToListAsync();

        return result;
    }

}
