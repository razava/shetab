using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
