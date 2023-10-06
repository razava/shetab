using Domain.Models.Relational.IdentityAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    public Task<ApplicationUser> GetOrCreateCitizen(string phoneNumber, string firstName, string lastName);
    
}
