using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class QuickAccessRepository : GenericRepository<QuickAccess>, IQuickAccessRepository
{
    public QuickAccessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
