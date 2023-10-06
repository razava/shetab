using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class MediaRepository: GenericRepository<Media>, IMediaRepository
{
    public MediaRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
