using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.RegisterCitizenCommand;

internal class RegisterCitizenCommandHandler : IRequestHandler<RegisterCitizenCommand, bool>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICaptchaProvider _captchaProvider;
    private readonly ISmsService _smsService;

    public RegisterCitizenCommandHandler(IAuthenticationService authenticationService, ICaptchaProvider captchaProvider, ISmsService smsService)
    {
        _authenticationService = authenticationService;
        _captchaProvider = captchaProvider;
        _smsService = smsService;
    }
    public async Task<bool> Handle(RegisterCitizenCommand request, CancellationToken cancellationToken)
    {
        if (request.CaptchaValidateModel is not null)
        {
            var isCaptchaValid = _captchaProvider.Validate(request.CaptchaValidateModel);
            if (!isCaptchaValid)
            {
                throw new InvalidCaptchaException();
            }
        }

        bool result;
        try
        {
            result = await _authenticationService.RegisterCitizen(request.Username, request.Password);
            if (result)
            {
                var verificationCode = await _authenticationService.GetVerificationCode(request.Username);
                try
                {
                    await _smsService.SendVerificationAsync(request.Username, verificationCode);
                }
                catch
                {
                    throw new SendSmsException();
                }
                result = true;
            }
        }
        catch
        {
            throw new Exception();
        }

        return result;
    }
}
