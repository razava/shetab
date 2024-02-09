using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Commands.AddFormCommand;

public sealed record AddFormCommand(
    int InstanceId,
    string Title,
    List<FormElement> Elements) : IRequest<Result<Form>>;