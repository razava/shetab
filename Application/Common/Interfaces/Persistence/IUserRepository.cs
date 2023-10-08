using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    public Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName);
    public Task<List<ApplicationUser>> GetUserActors();
    public Task<List<ApplicationRole>> GetRoleActors();
    public Task<List<ApplicationUser>> GetUsersInRole(string roleName);
    public Task<List<ApplicationRole>> GetRoles();
    public Task<List<Actor>> GetActors();
    
}
