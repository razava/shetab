using Domain.Models.Relational;

namespace Application.Common.Interfaces.Persistence;

public interface ICategoryRepository : IGenericRepository<Category>
{
    public Task<Category?> GetByIDAsync(int id, bool trackChanges = true);
    public Task<Category?> GetById(int id, string includeProperties = "", bool trackChanges = false);
    public Task<Category?> GetStaffCategories(int instanceId, string userId, List<string> roles);
    public Task<Category?> GetCategories(int instanceId, List<string>? Roles, bool ReturnAll = false);
}
