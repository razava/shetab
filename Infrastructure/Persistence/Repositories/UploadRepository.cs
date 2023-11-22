using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Infrastructure.Persistence.Repositories;

public class UploadRepository : GenericRepository<Upload>, IUploadRepository
{
    public UploadRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
