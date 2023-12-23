using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ActorRepository : GenericRepository<Actor>, IActorRepository
{
    public ActorRepository(
        ApplicationDbContext dbContext) : base(dbContext)
    {       
    }

    public async Task<List<IsInRegionModel>> GetUserRegionsAsync(int instanceId, string userId)
    {
        var instance = await context.Set<ShahrbinInstance>()
            .Where(si => si.Id == instanceId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        if (instance is null)
            throw new InstanceNotFoundException();
        var regions = await context.Set<Region>()
            .Where(r => r.CityId == instance.CityId)
            .AsNoTracking()
            .ToListAsync();

        var actorRegionIds = await context.Set<Actor>()
            .Where(a => a.Identifier == userId)
            .Select(a => a.Regions.Select(r => r.Id))
            .SingleOrDefaultAsync();
        if (actorRegionIds is null)
            throw new ActorNotFoundException();

        var result = new List<IsInRegionModel>();
        foreach (var region in regions)
        {
            if (actorRegionIds.Contains(region.Id))
            {
                result.Add(new IsInRegionModel(region.Id, region.Name, true));
            }
            else
            {
                result.Add(new IsInRegionModel(region.Id, region.Name, false));
            }
        }

        return result;
    }

    public async Task<bool> UpdateUserRegionsAsync(int instanceId, string userId, List<IsInRegionModel> userRegions)
    {
        var instance = await context.Set<ShahrbinInstance>()
            .Where(si => si.Id == instanceId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        if (instance is null)
            throw new InstanceNotFoundException();
        var regions = await context.Set<Region>()
            .Where(r => r.CityId == instance.CityId)
            .ToListAsync();

        var actor = await context.Set<Actor>()
            .Where(a => a.Identifier == userId)
            .Include(a => a.Regions)
            .SingleOrDefaultAsync();
        if (actor is null)
            throw new ActorNotFoundException();

        actor.Regions.Clear();
        foreach (var userRegion in userRegions)
        {
            var region = regions.Where(r => r.Id == userRegion.RegionId).SingleOrDefault();
            if (region is not null)
            {
                actor.Regions.Add(region);
            }
            else
            {
                //region is invalid, ignore!
            }
        }

        return true;
    }
}
