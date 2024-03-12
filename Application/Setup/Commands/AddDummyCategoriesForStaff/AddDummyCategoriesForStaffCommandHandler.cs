using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;

namespace Application.Setup.Commands.AddDummyCategoriesForStaff;

internal class AddDummyCategoriesForStaffCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<AddDummyCategoriesForStaffCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDummyCategoriesForStaffCommand request, CancellationToken cancellationToken)
    {
        var allRoles = await userRepository.GetRoles();
        var clerkRole = allRoles.Where(r => r.Name == RoleNames.Clerk).FirstOrDefault();
        if (clerkRole is null)
        {
            clerkRole = new Domain.Models.Relational.IdentityAggregate.ApplicationRole()
            {
                Name = RoleNames.Clerk,
                Title = "کارمند",
            };
            await userRepository.CreateRoleAsync(clerkRole);
        }

        return true;
    }
}
