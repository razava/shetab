using Domain.Models.Relational.ProcessAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IProcessRepository : IGenericRepository<Process>
{
    public Task<Process?> GetByIDAsync(int id, bool trackChanges = true);
    public Task<Process?> AddTypicalProcess(int instanceId, string code, string title, List<int> actorIds);
    public Task<Process?> UpdateTypicalProcess(int id, string code, string title, List<int> actorIds);
}
