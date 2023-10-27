using Domain.Models.Relational;

namespace Application.Common.Interfaces.Persistence;

public interface IReportRepository : IGenericRepository<Report>
{
    public Task<Report?> GetByIDAsync(Guid id, bool trackChanges = true);
}
