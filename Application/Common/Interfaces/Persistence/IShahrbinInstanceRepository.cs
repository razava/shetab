using Domain.Models.Relational.Common;

namespace Application.Common.Interfaces.Persistence;

public interface IShahrbinInstanceRepository : IGenericRepository<ShahrbinInstance>
{
    public Task<ShahrbinInstance> GetById(int instanceId);
}
