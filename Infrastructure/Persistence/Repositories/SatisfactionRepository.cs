using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class SatisfactionRepository : GenericRepository<Satisfaction>, ISatisfactionRepository
{
    public SatisfactionRepository(
        ApplicationDbContext dbContext) : base(dbContext)
    {       
    }
}
