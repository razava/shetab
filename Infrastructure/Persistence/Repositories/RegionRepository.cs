using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RegionRepository : GenericRepository<Region>, IRegionRepository
{
    private readonly ApplicationDbContext _dbContext;
    public RegionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Region>> GetRegionsByCityId(int id)
    {
        var result = await _dbContext.Region.AsNoTracking()
            .Where(r => r.CityId == id)
            .ToListAsync();

        return result;
    }
}
