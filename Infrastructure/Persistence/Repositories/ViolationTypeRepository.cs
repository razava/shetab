using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class ViolationTypeRepository : GenericRepository<ViolationType>, IViolationTypeRepository
{
    public ViolationTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
