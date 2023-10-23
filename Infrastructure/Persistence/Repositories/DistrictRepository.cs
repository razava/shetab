using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class DistrictRepository : GenericRepository<District>, IDistrictRepository
{
    public DistrictRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
