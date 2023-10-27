using Domain.Models.Relational.ProcessAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IProcessRepository : IGenericRepository<Process>
{
    public Task<Process?> GetByIDAsync(int id, bool trackChanges = true);
}
