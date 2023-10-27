using Domain.Models.Relational;

namespace Application.Common.Interfaces.Persistence;

public interface ICategoryRepository : IGenericRepository<Category>
{
    public Task<Category?> GetByIDAsync(int id, bool trackChanges = true);
}
