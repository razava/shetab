using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ReportNoteRepository : GenericRepository<ReportNote>, IReportNoteRepository
{
    public ReportNoteRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
