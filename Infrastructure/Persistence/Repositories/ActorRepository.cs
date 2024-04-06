using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;

namespace Infrastructure.Persistence.Repositories;

public class ActorRepository : GenericRepository<Actor>, IActorRepository
{
    public ActorRepository(
        ApplicationDbContext dbContext) : base(dbContext)
    {       
    }

    public async Task<Result<ActorIdentityResponse>> GetActorIdentityAsync(int actorId)
    {
        var actor = await context.Set<Actor>()
            .Where(a => a.Id == actorId)
            .Select(a =>
                a.Type == ActorType.Person ? context.Set<ApplicationUser>().Where(u => u.Id == a.Identifier)
                    .Select(u => new ActorIdentityResponse(a.Type, u.Title, u.FirstName, u.LastName)).FirstOrDefault() :
                a.Type == ActorType.Role ? context.Set<ApplicationRole>().Where(r => r.Id == a.Identifier)
                    .Select(u => new ActorIdentityResponse(a.Type, u.Title, "", "")).FirstOrDefault() :
                new ActorIdentityResponse(a.Type, "ارجاع خودکار", "", ""))
            .FirstOrDefaultAsync();

        if (actor is null)
            return NotFoundErrors.Actor;

        return actor;
    }

    public async Task<Result<List<IsInRegionModel>>> GetUserRegionsAsync(int instanceId, string userId)
    {
        var instance = await context.Set<ShahrbinInstance>()
            .Where(si => si.Id == instanceId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        if (instance is null)
            return NotFoundErrors.Instance;
        var regions = await context.Set<Region>()
            .Where(r => r.CityId == instance.CityId)
            .AsNoTracking()
            .ToListAsync();

        var actorRegionIds = await context.Set<Actor>()
            .Where(a => a.Identifier == userId)
            .Select(a => a.Regions.Select(r => r.Id))
            .SingleOrDefaultAsync();
        if (actorRegionIds is null)
            return NotFoundErrors.Actor;

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

    public async Task<Result<bool>> UpdateUserRegionsAsync(int instanceId, string userId, List<IsInRegionModel> userRegions)
    {
        var instance = await context.Set<ShahrbinInstance>()
            .Where(si => si.Id == instanceId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        if (instance is null)
            return NotFoundErrors.Instance;
        var regions = await context.Set<Region>()
            .Where(r => r.CityId == instance.CityId)
            .ToListAsync();

        var actor = await context.Set<Actor>()
            .Where(a => a.Identifier == userId)
            .Include(a => a.Regions)
            .SingleOrDefaultAsync();
        if (actor is null)
            return NotFoundErrors.Actor;

        actor.Regions.Clear();
        foreach (var userRegion in userRegions)
        {
            if (userRegion.IsIn)
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
        }

        return true;
    }
}
