using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
