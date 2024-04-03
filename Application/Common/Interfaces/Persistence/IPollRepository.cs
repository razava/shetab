using Application.Info.Common;
using Domain.Models.Relational.PollAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IPollRepository
{
    public void Add(Poll poll);
    public void Update(Poll poll);
    public Task<Poll> GetById(int id, string userId);
    public Task<Poll> GetByIdNoTracking(int id, string userId);
    public Task<Poll> GetById(int id);
    public Task<InfoModel> GetPollResult(int id);
    public Task<List<Poll>> GetAll(string userId, bool returnAll = false);
}
