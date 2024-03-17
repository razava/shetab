namespace Application.Forms.Commands.DeleteForm;

public sealed record DeleteFormCommand(
    Guid Id) : IRequest<Result<bool>>;