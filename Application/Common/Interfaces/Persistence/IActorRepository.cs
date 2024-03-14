using Application.Users.Common;
using Domain.Models.Relational.ProcessAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IActorRepository : IGenericRepository<Actor>
{
    public Task<Result<List<IsInRegionModel>>> GetUserRegionsAsync(int instanceId, string userId);
    public Task<Result<bool>> UpdateUserRegionsAsync(int instanceId, string userId, List<IsInRegionModel> regions);
}
