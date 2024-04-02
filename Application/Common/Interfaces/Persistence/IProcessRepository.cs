using Domain.Models.Relational.ProcessAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IProcessRepository : IGenericRepository<Process>
{
    public Task<T?> GetProcessById<T>(int id, Expression<Func<Process, T>> selector);
    public Task<List<T>> GetProcesses<T>(
        int instanceId, Expression<Func<Process, T>> selector, Expression<Func<Process, bool>> filter);
    public Task<Process?> GetByIDAsync(int id, bool trackChanges = true);
    public Task<Process?> AddTypicalProcess(int instanceId, string code, string title, List<int> actorIds);
    public Task<Process?> UpdateTypicalProcess(int id, string code, string title, List<int> actorIds);
    public Task<bool> LogicalDelete(int id);
}
