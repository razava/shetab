using Application.Common.Interfaces.Security;

namespace Application.Authentication.Commands.RegisterCitizenCommand;

internal class LogisterCitizenCommandHandler(
    IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider) 
    : IRequestHandler<LogisterCitizenCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        LogisterCitizenCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                return AuthenticationErrors.InvalidCaptcha;
            }
        }

        var tokenResult = await authenticationService.LogisterCitizen(request.PhoneNumber);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult();
        
        return tokenResult.Value.Token;
    }
}