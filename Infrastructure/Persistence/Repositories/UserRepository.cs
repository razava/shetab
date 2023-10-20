using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : base(dbContext)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName)
    {
        var user = await _userManager.FindByNameAsync(phoneNumber);
        if (user == null)
        {
            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                FirstName = firstName,
                LastName = lastName
            };
            //TODO: Generate password randomly
            var password = "aA@12345";
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("User creation failed.", null);

            var result2 = await _userManager.AddToRoleAsync(user, "Citizen");

            if (!result2.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception("Role assignment failed.", null);
            }

            //TODO: Send the user its credentials
        }

        return user;
    }

    public async Task<List<ApplicationRole>> GetRoleActors()
    {
        var roleActorsIdentifier = await _dbContext.Actor
            .Where(p => p.Type == ActorType.Role)
            .Select(p => p.Identifier)
            .AsNoTracking()
            .ToListAsync();

        var result = await _dbContext.Roles.Where(p => roleActorsIdentifier.Contains(p.Id)).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<ApplicationUser>> GetUserActors()
    {
        var userActorsIdentifier = await _dbContext.Actor
            .Where(p => p.Type == ActorType.Person)
            .Select(p => p.Identifier)
            .AsNoTracking()
            .ToListAsync();

        var result = await _dbContext.Users.Where(p => userActorsIdentifier.Contains(p.Id)).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<ApplicationRole>> GetRoles()
    {
        var result = await _dbContext.Roles.ToListAsync();
        return result;
    }

    public async Task<List<ApplicationUser>> GetUsersInRole(string roleName)
    {
        var result = await _dbContext.Roles.Where(r => r.Name == roleName)
            .Join(_dbContext.UserRoles, r => r.Id, ur => ur.RoleId, (r, ur) => new { ur.UserId })
            .Join(_dbContext.Users, uid => uid.UserId, u => u.Id, (uid, u) => u )
            .ToListAsync();
        //var roleId = await _dbContext.Roles.Where(p => p.Name == roleName).Select(p => p.Id).FirstOrDefaultAsync();
        //var userIds = await _dbContext.UserRoles.Where(p => p.RoleId == roleId).Select(p => p.UserId).ToListAsync();
        //var result = await _dbContext.Users.Where(p => userIds.Contains(p.Id)).Include(p => p.Contractors).Include(p => p.Executeves).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<List<Actor>> GetActors()
    {
        var result = await _dbContext.Actor.Include(p => p.Regions)
            .Include(p => p.Stages)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }

    public async Task<IdentityResult> AddToRolesAsync(ApplicationUser user, string[] roles)
    {
        var result = await _userManager.AddToRolesAsync(user, roles);
        return result;
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        return result;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string username)
    {
        var result = await _userManager.FindByNameAsync(username);
        return result;
    }

    public async Task<ApplicationRole?> FindRoleByNameAsync(string roleName)
    {
        var result = await _roleManager.FindByNameAsync(roleName);
        return result;
    }

    public async Task<IdentityResult> CreateRoleAsync(ApplicationRole applicationRole)
    {
        var result = await _roleManager.CreateAsync(applicationRole);
        return result;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);
        return result;
    }
}
