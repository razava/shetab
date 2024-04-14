using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Users.Commands.CreateNewPassword;

internal class CreateNewPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateNewPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateNewPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepository.CreateNewPasswordAsync(request.UserId, request.Password);
        if (!result)
            return AuthenticateErrors.ChangePasswordFailed;

        return ResultMethods.GetResult(result, UpdateSuccess.Password);
    }
}
