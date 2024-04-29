
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational.IdentityAggregate;
using System.Security.Claims;

namespace Application.Setup.Commands.AddComplaintRoles;

internal sealed class AddComplaintRolesCommandHandler(IUserRepository userRepository) : IRequestHandler<AddComplaintRolesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddComplaintRolesCommand request, CancellationToken cancellationToken)
    {
        //Init roles
        var rolesInfo = new List<Tuple<string, string, List<string>>>()
        {
            //todo : claims not compelete.
                new Tuple<string, string, List<string>>("ComplaintAdmin", "مدیریت شکایات",
                new List<string>
                {
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                    AppClaimTypes.User.Create,
                }),
                new Tuple<string, string, List<string>>("ComplaintInspector", "بازرس شکایات",
                new List<string>
                {
                    AppClaimTypes.Account.Get,
                    AppClaimTypes.Account.Update,
                })
        };

        var roles = new List<ApplicationRole>();
        foreach (var role in rolesInfo)
        {
            if (!await userRepository.RoleExistsAsync(role.Item1))
            {
                var r = new ApplicationRole() { Name = role.Item1, Title = role.Item2 };
                await userRepository.CreateRoleAsync(r);
                foreach (var claim in role.Item3)
                {
                    await userRepository.AddClaimsToRoleAsunc(r, new Claim(claim, "TRUE"));
                }
                roles.Add(r);
            }
        }

        //init default users
        ApplicationUser user;
        if (await userRepository.FindByNameAsync("ComplaintAdmin") is null)
        {
            user = new ApplicationUser() {
                UserName = "ComplaintAdmin",
                Title = "مدیر شکایات",
                PhoneNumberConfirmed = true };
            await userRepository.CreateAsync(user, "aA@12345");
            await userRepository.AddToRoleAsync(user, "ComplaintAdmin");
        }

        return true;
    }
}
