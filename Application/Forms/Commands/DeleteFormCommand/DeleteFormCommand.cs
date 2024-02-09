namespace Application.Forms.Commands.DeleteFormCommand;

public sealed record DeleteFormCommand(
    Guid Id) : IRequest<Result<bool>>;