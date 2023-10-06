using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : base(dbContext)
    {
        _userManager = userManager;
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
}
