using Application.Forms.Common;

namespace Application.Forms.Commands.UpdateForm;

public sealed record UpdateFormCommand(
    Guid Id,
    string? Title,
    List<FormElementModel>? Elements) : IRequest<Result<bool>>;
