using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Authentication.Commands.LoginCommand;

public sealed record ChangePasswordCommand(string Username, string OldPassword, string NewPassword, CaptchaValidateModel? CaptchaValidateModel = null):IRequest<bool>;
