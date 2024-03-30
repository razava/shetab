using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class OrganizationalUnitRepository : GenericRepository<OrganizationalUnit>, IOrganizationalUnitRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    public OrganizationalUnitRepository(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager) : base(dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<T?> GetOrganizationalUnitById<T>(int id, Expression<Func<OrganizationalUnit, T>> selector)
    {
        var result = await _dbContext.OrganizationalUnit.AsNoTracking()
            .Where(ou => ou.Id == id)
            .Select(selector)
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<T?> GetOrganizationalUnitByUserId<T>(
        string userId,
        Expression<Func<OrganizationalUnit, T>> selector)
    {
        var result = await _dbContext.OrganizationalUnit.AsNoTracking()
            .Where(ou => ou.UserId == userId)
            .Select(selector)
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<List<T>> GetOrganizationalUnits<T>(
        int instanceId,
        Expression<Func<OrganizationalUnit, bool>>? filter,
        Expression<Func<OrganizationalUnit, T>> selector)
    {
        var result = await _dbContext.OrganizationalUnit.AsNoTracking()
            .Where(ou => ou.ShahrbinInstanceId == instanceId && ou.Type == OrganizationalUnitType.OrganizationalUnit)
            .Where(filter)
            .Select(selector)
            .ToListAsync();

        return result;
    }

    public async Task<bool> PhysicalDelete(int id)
    {
        var organizationalUnit = await context.OrganizationalUnit.Where(o => o.Id == id).SingleOrDefaultAsync();
        if(organizationalUnit == null) { throw new NotFoundException("واحد سازمانی"); }

        var hasParentDependency = await context.OrganizationalUnit.AnyAsync(e => e.OrganizationalUnits.Contains(organizationalUnit));
        if (hasParentDependency) { throw new BadRequestException("واحد سازمانی وابستگی دارد."); }

        var hasChildtDependency = await context.OrganizationalUnit.AnyAsync(e => e.ParentOrganizationalUnits.Contains(organizationalUnit));
        if (hasChildtDependency) { throw new BadRequestException("واحد سازمانی وابستگی دارد."); }

        var user = await _userManager.FindByIdAsync(organizationalUnit.UserId);

        context.OrganizationalUnit.Remove(organizationalUnit);

        if(user != null)
        {
            var r = await _userManager.DeleteAsync(user);
        }

        return true;
    }
}
