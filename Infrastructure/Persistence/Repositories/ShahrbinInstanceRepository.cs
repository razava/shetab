using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class ShahrbinInstanceRepository : GenericRepository<ShahrbinInstance>, IShahrbinInstanceRepository
{
    public ShahrbinInstanceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
