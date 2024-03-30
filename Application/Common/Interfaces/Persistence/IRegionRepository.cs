using Domain.Models.Relational.Common;

namespace Application.Common.Interfaces.Persistence;

public interface IRegionRepository : IGenericRepository<Region>
{
    public Task<List<Region>> GetRegionsByCityId(int id);
}
