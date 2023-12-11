using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class NewsRepository : GenericRepository<News>, INewsRepository
{
    public NewsRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
