using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;

namespace Application.Users.Commands.CreateContractor;

internal class CreateContractorCommandHandler(
    IUserRepository userRepository) : IRequestHandler<CreateContractorCommand, Result<ApplicationUser>>
{
    
    public async Task<Result<ApplicationUser>> Handle(CreateContractorCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserRoles.Contains(RoleNames.Executive))
        {
            return AccessDeniedErrors.Executive;
        }
        var result = await userRepository.AddContractorAsync(
            request.ExecutiveId,
            request.PhoneNumber,
            request.FirstName,
            request.LastName,
            request.Title,
            request.Organization);

        return result;
    }
}
