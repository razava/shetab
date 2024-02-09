using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Commands.UpdateFormCommand;

public sealed record UpdateFormCommand(
    Guid Id,
    string? Title,
    List<FormElement>? Elements) : IRequest<Result<Form>>;