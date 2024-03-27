using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<FeedbackWithReciepient>> GetToSendFeedbacks(int count)
    {
        var result = await context.Set<Feedback>()
            .Where(f => f.LastSent == null)
            .Take(count)
            .Select(f => new FeedbackWithReciepient(
                f.Id,
                f.User.PhoneNumber ?? ""))
            .ToListAsync();

        return result;
    }

    public async Task SetAsSent(List<Guid> ids)
    {
        var now = DateTime.UtcNow;

        await context.Set<Feedback>()
            .ExecuteUpdateAsync(setter =>setter
                .SetProperty(f => f.LastSent, now)
                .SetProperty(f => f.TryCount, f => f.TryCount + 1));
    }
}
