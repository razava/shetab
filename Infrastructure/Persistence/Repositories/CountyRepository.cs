using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class CountyRepository : GenericRepository<County>, ICountyRepository
{
    public CountyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
