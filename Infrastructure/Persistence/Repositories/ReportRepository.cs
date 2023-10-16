using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class ReportRepository: GenericRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
