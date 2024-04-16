﻿using Application.Common.Helper;
using Application.Common.Interfaces.Security;
using SharedKernel.Successes;

namespace Application.Authentication.Commands.ChangePhoneNumberCommand;

internal sealed class ChangePasswordCommandHandler(
    IAuthenticationService authenticationService) 
    : IRequestHandler<ChangePhoneNumberCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangePhoneNumberCommand request, CancellationToken cancellationToken)
    {

        var result = await authenticationService.ChangePhoneNumber(
            request.Username,
            request.OtpToken1,
            request.Code1,
            request.OtpToken2,
            request.Code2);
        if (result.IsFailed)
            return result.ToResult();
        
        return ResultMethods.GetResult(result.Value, UpdateSuccess.PhoneNumber);
    }
}
