using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using ErrorOr;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultModel>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;
    private readonly ISmsService _smsService;

    public LoginCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ISmsService smsService)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
        _smsService = smsService;
    }
    public async Task<LoginResultModel> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if(request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new Exception("Invalid captcha");
            }
        }

        LoginResultModel? result;
        try
        {
            result = await _authenticationService.Login(request.Username, request.Password);
        }
        catch (PhoneNumberNotConfirmedException)
        {
            var verificationCode = await _authenticationService.GetVerificationCode(request.Username);
            try
            {
                await _smsService.SendVerificationAsync(request.Username, verificationCode);
            }
            catch
            {
                throw new Exception("There was a problem in sending sms.");
            }
            result = new LoginResultModel("", true);
        }
        return result;
    }
}
