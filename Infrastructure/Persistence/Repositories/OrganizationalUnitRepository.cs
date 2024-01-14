using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class OrganizationalUnitRepository : GenericRepository<OrganizationalUnit>, IOrganizationalUnitRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    public OrganizationalUnitRepository(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager) : base(dbContext)
    {
        _userManager = userManager;
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
