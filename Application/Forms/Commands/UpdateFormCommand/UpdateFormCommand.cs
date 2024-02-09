using Application.Forms.Common;

namespace Application.Forms.Commands.UpdateFormCommand;

public sealed record UpdateFormCommand(
    Guid Id,
    string? Title,
    List<FormElementModel>? Elements) : IRequest<Result<FormResponse>>;
