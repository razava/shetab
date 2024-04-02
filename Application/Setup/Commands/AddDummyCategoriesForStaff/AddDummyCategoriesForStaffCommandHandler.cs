using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;

namespace Application.Setup.Commands.AddDummyCategoriesForStaff;

internal class AddDummyCategoriesForStaffCommandHandler(
    IUserRepository userRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
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
            var result = await userRepository.CreateRoleAsync(clerkRole);
            if (result.Succeeded)
            {
                var clerkRoleId = (await userRepository.FindRoleByNameAsync(RoleNames.Clerk))?.Id;
                var citizenRoleId = (await userRepository.FindRoleByNameAsync(RoleNames.Citizen))?.Id;
                var oldRoot = await categoryRepository.GetSingleAsync(
                        c => c.ParentId == null &&
                        c.CategoryType == Domain.Models.Relational.Common.CategoryType.Root
                        , true);

                if (clerkRoleId == null || citizenRoleId == null || oldRoot == null) { return new Error("internal server error"); }

                var newRoot = new Category()
                {
                    ShahrbinInstanceId = request.instanceId,
                    Title = "ریشه",
                    Order = 0,
                    Duration = 0,
                    Code = "",
                    CategoryType = Domain.Models.Relational.Common.CategoryType.Root,
                    RoleId = citizenRoleId!
                };

                categoryRepository.Insert(newRoot);
                await unitOfWork.SaveAsync();

                if(newRoot.Id != 0)
                {
                    var clerkRoot = new Category()
                    {
                        ShahrbinInstanceId = request.instanceId,
                        Title = "کارمند ها",
                        Order = 0,
                        Duration = 0,
                        Code = "",
                        CategoryType = Domain.Models.Relational.Common.CategoryType.Root,
                        RoleId = clerkRoleId!,
                        ParentId = newRoot.Id
                    };

                    categoryRepository.Insert(clerkRoot);

                    oldRoot.Title = "شهروندان";
                    oldRoot.ParentId = newRoot.Id;
                    categoryRepository.Update(oldRoot);

                    await unitOfWork.SaveAsync();

                }


            }
                

        }



        return true;
    }
}
