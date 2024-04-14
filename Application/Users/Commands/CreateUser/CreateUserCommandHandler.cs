using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Users.Queries.GetUserById;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Successes;

namespace Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateUserCommand, Result<AdminGetUserDetailsResponse>>
{

    public async Task<Result<AdminGetUserDetailsResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            ShahrbinInstanceId = request.InstanceId,
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Title = request.Title,
            PhoneNumberConfirmed = true
        };
        await userRepository.CreateAsync(user, request.Password);

        if (request.Roles is not null)
        {
            request.Roles.RemoveAll(r => r == RoleNames.PowerUser || r == RoleNames.GoldenUser);
            await userRepository.AddToRolesAsync(user, request.Roles.ToArray());

            var needActorRoles = new List<string>() { RoleNames.Operator, RoleNames.Executive, RoleNames.Mayor };
            if (request.Roles.Any(needActorRoles.Contains))
            {
                var regions = new List<Region>();
                if(request.Regions is not null)
                {
                    regions = await unitOfWork.DbContext.Set<Region>().Where(r => request.Regions.Contains(r.Id)).ToListAsync();
                }
                
                var actor = new Actor() { Identifier = user.Id, Type = ActorType.Person, Regions = regions };
                unitOfWork.DbContext.Add(actor);
                await unitOfWork.SaveAsync();
            }
        }

        
        return ResultMethods.GetResult(user.Adapt<AdminGetUserDetailsResponse>(), CreationSuccess.User);
    }
}
