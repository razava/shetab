using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ShahrbinInstanceRepository : GenericRepository<ShahrbinInstance>, IShahrbinInstanceRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ShahrbinInstanceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShahrbinInstance> GetById(int instanceId)
    {
        var result = await _dbContext.ShahrbinInstance.AsNoTracking()
            .Where(s => s.Id == instanceId)
            .SingleOrDefaultAsync();

        if(result == null)
        {
            throw new ServerNotFoundException("خطایی رخ داد", new ShahrbinInstanceNotFoundException());
        }

        return result;
    }
}
