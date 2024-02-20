using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Common.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    public Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName);
    public Task<List<ApplicationUser>> GetUserActors();
    public Task<List<ApplicationUser>> GetUserActors(List<string> ids);
    public Task<List<ApplicationRole>> GetRoleActors();
    public Task<List<ApplicationRole>> GetRoleActors(List<string> ids);
    public Task<List<ApplicationUser>> GetUsersInRole(string roleName);
    public Task<List<string>> GetUserRoles(string userId);
    public Task<List<ApplicationRole>> GetRoles();
    public Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    public Task<List<Actor>> GetActors();
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    public Task<ApplicationUser> AddContractorAsync(
        string executiveId,
        string phoneNumber,
        string firstName,
        string lastName,
        string title,
        string organization);
    public Task<IdentityResult> DeleteAsync(ApplicationUser user);

    public Task<IdentityResult> AddToRolesAsync(ApplicationUser user, string[] roles);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    public Task<bool> UpdateRolesAsync(string userId, List<string> roles);
    public Task<ApplicationUser?> FindByNameAsync(string username);
    public Task<ApplicationRole?> FindRoleByNameAsync(string roleName);
    public Task<IdentityResult> CreateRoleAsync(ApplicationRole applicationRole);
    public Task<IdentityResult> AddClaimsToRoleAsunc(ApplicationRole role, Claim claim);
    public Task<bool> RoleExistsAsync(string roleName);
    public Task<List<Actor>> GetActorsAsync(string userId);
    public Task<bool> CreateNewPasswordAsync(string userId, string password);
    public Task<IdentityResult> RegisterWithRoleAsync(ApplicationUser user, string password, string role);
    public Task<bool> UpdateCategoriesAsync(string userId, List<int> categoryIds);
    public Task<List<int>> GetUserCategoriesAsync(string userId);
}
