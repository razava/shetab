using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class ProvinceRepository: GenericRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
