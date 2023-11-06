using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class ViolationRepository : GenericRepository<Violation>, IViolationRepository
{
    public ViolationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
