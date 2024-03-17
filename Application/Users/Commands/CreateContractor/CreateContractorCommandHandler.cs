using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Mapster;

namespace Application.Users.Commands.CreateContractor;

internal class CreateContractorCommandHandler(
    IUserRepository userRepository) : IRequestHandler<CreateContractorCommand, Result<GetContractorsListResponse>>
{
    
    public async Task<Result<GetContractorsListResponse>> Handle(CreateContractorCommand request, CancellationToken cancellationToken)
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

        return result.Adapt<GetContractorsListResponse>();
    }
}
