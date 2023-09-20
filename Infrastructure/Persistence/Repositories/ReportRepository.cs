using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class ReportRepository: GenericRepository<Report>
{
    public ReportRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
