using Domain.Models.Relational.IdentityAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IReportLikesRepository
{
    public Task<bool> Like(string userId, Guid reportId, bool? isLiked = null);
    public Task<int> Count(Guid reportId);
}
