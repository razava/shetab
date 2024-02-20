using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICommunicationService _communicationService;

    public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ICommunicationService communicationService) : base(dbContext)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _communicationService = communicationService;
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
                LastName = lastName,
                TwoFactorEnabled = true
            };
            //TODO: Generate password randomly
            var password = "aA@12345";
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new CreationFailedException("کاربر");

            var result2 = await _userManager.AddToRoleAsync(user, "Citizen");

            if (!result2.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new RoleAssignmentFailedException((result2.Errors.Any()) ? result2.Errors.ToList() : null);
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

    public async Task<List<ApplicationRole>> GetRoleActors(List<string> ids)
    {
        var result = await _dbContext.Roles.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync();
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

    public async Task<List<ApplicationUser>> GetUserActors(List<string> ids)
    {
        var result = await _dbContext.Users.Where(p => ids.Contains(p.Id)).AsNoTracking().ToListAsync();
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
        if (!result.Succeeded)
        {
            throw new UserCreationFailedException(result.Errors.ToList());
        }
            
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
        if (!result.Succeeded)
            throw new RoleAssignmentFailedException(result.Errors.ToList());
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


    public async Task<IdentityResult> AddClaimsToRoleAsunc(ApplicationRole role, Claim claim)
    {
        var result = await _roleManager.AddClaimAsync(role, claim);
        return result;
    }
        


    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);
        return result;
    }

    public async Task<List<Actor>> GetActorsAsync(string userId)
    {
        var identifiers = await _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        identifiers.Add(userId);
        var result = await _dbContext.Actor.Where(a => identifiers.Contains(a.Identifier)).ToListAsync();
        return result;
    }

    public async Task<bool> CreateNewPasswordAsync(string userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("کاربر");
        var result = await _userManager.RemovePasswordAsync(user);
        if (result is null || !result.Succeeded)
            return false;
        result = await _userManager.AddPasswordAsync(user, password);
        if (result is null || !result.Succeeded)
            return false;
        return true;
    }

    public async Task<bool> UpdateRolesAsync(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("کاربر");
        var currentRoles = await _userManager.GetRolesAsync(user);
        var inRoles = roles.Where(r => !currentRoles.Contains(r)).ToList();
        var outRoles = currentRoles.Where(r => !roles.Contains(r)).ToList();
        var result = await _userManager.RemoveFromRolesAsync(user, outRoles);
        if (result is null || !result.Succeeded)
            return false;
        var result2 = await _userManager.AddToRolesAsync(user, inRoles);
        if(result2 is null || !result2.Succeeded)
        {
            //try to rollback operation
            //TODO: This won't work in all cases. What if these commands fail?
            await _userManager.AddToRolesAsync(user, outRoles);
            return false;
        }
            
        return true;
    }

    public async Task<List<string>> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("کاربر");
        var roles = await _userManager.GetRolesAsync(user);
        if (roles is null)
            throw new NullUserRolesException();
        return roles.ToList();
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.IsInRoleAsync(user, role);
        return result;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);
        return result;
    }

    public async Task<IdentityResult> RegisterWithRoleAsync(ApplicationUser user, string password, string role)
    {
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            throw new UserCreationFailedException(createResult.Errors.ToList());
        }

        var assignRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!assignRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            throw new RoleAssignmentFailedException(assignRoleResult.Errors.ToList());
        }
            

        return createResult;
    }


    public async Task<bool> UpdateCategoriesAsync(string userId, List<int> categoryIds)
    {
        var user = await _dbContext.Set<ApplicationUser>()
            .Where(u => u.Id == userId)
            .Include(u => u.Categories)
            .SingleOrDefaultAsync();

        if (user is null)
            throw new NotFoundException("کاربر");

        var categories = new List<Category>();
        foreach (var categoryId in categoryIds)
        {
            var category = await _dbContext.Set<Category>().Where(c => c.Id == categoryId)/*.Include(c => c.Users)*/.SingleOrDefaultAsync();
            if (category is null) throw new NotFoundException("دسته بندی");
            categories.Add(category);
        }

        user.Categories = categories;
        
        return true;
    }


    public async Task<List<int>> GetUserCategoriesAsync(string userId)
    {
        var user = await _dbContext.Set<ApplicationUser>()
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Include(u => u.Categories)
            .SingleOrDefaultAsync();

        if (user is null)
            throw new NotFoundException("کاربر");

        return user.Categories.Select(u => u.Id).ToList();

    }

    public async Task<ApplicationUser> AddContractorAsync(string executiveId, string phoneNumber, string firstName, string lastName, string title, string organization)
    {
        var contractorUsername = "c-" + phoneNumber;
        var executive = await context.Set<ApplicationUser>()
            .Where(u => u.Id == executiveId)
            .Include(u => u.Contractors.Where(c => c.UserName == contractorUsername))
            .SingleOrDefaultAsync();
        if (executive is null)
            throw new Exception("Executive not found.");

        var contractor = await context.Set<ApplicationUser>()
            .Where(u => u.UserName == contractorUsername)
            .SingleOrDefaultAsync();

        if (contractor is null)
        {
            contractor = new ApplicationUser()
            {
                UserName = contractorUsername,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Organization = organization,
                Title = title,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var password = $"c@{Random.Shared.Next(10000, 99999)}C";
            var r = await RegisterWithRoleAsync(contractor, password, RoleNames.Contractor);

            context.Add(new Actor()
            {
                Identifier = contractor.Id,
                Type = ActorType.Person,
            });
            
            executive.Contractors.Add(contractor);

            var message = "پیمانکار گرامی، عضویت شما در سامانه شهربین با موفقیت انجام شد.";
            message += "\r\n";
            message += "لطفاً برای ورود از داده های زیر استفاده نمایید:\r\n";
            message += $"نام کاربری:{"c-" + phoneNumber}\r\nرمزعبور:{password}";
            await _communicationService.SendAsync(phoneNumber, message);
        }
        else
        {
            if (!executive.Contractors.Any())
                executive.Contractors.Add(contractor);
        }

        await context.SaveChangesAsync();

        return contractor;
    }
}
