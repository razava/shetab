using Application.Authentication.Commands.LoginCommand;
using FluentValidation;

namespace Application.Common.Validators;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.NewPassword).NotEmpty().MinimumLength(6);
    }
}
