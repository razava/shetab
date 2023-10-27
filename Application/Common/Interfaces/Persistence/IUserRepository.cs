using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.AspNetCore.Identity;
using static Application.Setup.Commands.AddInstanceCommandHandler;

namespace Application.Common.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    public Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName);
    public Task<List<ApplicationUser>> GetUserActors();
    public Task<List<ApplicationUser>> GetUserActors(List<string> ids);
    public Task<List<ApplicationRole>> GetRoleActors();
    public Task<List<ApplicationRole>> GetRoleActors(List<string> ids);
    public Task<List<ApplicationUser>> GetUsersInRole(string roleName);
    public Task<List<ApplicationRole>> GetRoles();
    public Task<List<Actor>> GetActors();
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

    public Task<IdentityResult> AddToRolesAsync(ApplicationUser user, string[] roles);
    public Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    public Task<ApplicationUser?> FindByNameAsync(string username);
    public Task<ApplicationRole?> FindRoleByNameAsync(string roleName);
    public Task<IdentityResult> CreateRoleAsync(ApplicationRole applicationRole);
    public Task<bool> RoleExistsAsync(string roleName);
}
