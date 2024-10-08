﻿using Application.Authentication.Common;
using Application.Common.Helper;
using Application.Common.Interfaces.Security;
using SharedKernel.Successes;

namespace Application.Authentication.Queries.ChangePhoneNumberQuery;

internal sealed class ChangePhoneNumberQueryHandler(
    IAuthenticationService authenticationService,
    ICaptchaProvider captchaProvider) 
    : IRequestHandler<ChangePhoneNumberQuery, Result<ChangePhoneNumberQueryResponse>>
{
    public async Task<Result<ChangePhoneNumberQueryResponse>> Handle(
        ChangePhoneNumberQuery request,
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

        var tokenResult = await authenticationService.RequestToChangePhoneNumber(
            request.Username, request.NewPhoneNumber);
        if (tokenResult.IsFailed)
            return tokenResult.ToResult();
        var result = tokenResult.Value;

        return ResultMethods.GetResult(
            new ChangePhoneNumberQueryResponse(
            result.Token1.PhoneNumber, result.Token1.Token,
            result.Token2.PhoneNumber, result.Token2.Token),
            CreationSuccess.PhoneNumber);
    }
}
