using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class RegionRepository : GenericRepository<Region>, IRegionRepository
{
    public RegionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
