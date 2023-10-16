using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ProcessRepository : GenericRepository<Process>, IProcessRepository
{
    public ProcessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
