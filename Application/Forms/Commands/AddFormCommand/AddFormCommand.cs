using Application.Forms.Common;

namespace Application.Forms.Commands.AddFormCommand;

public sealed record AddFormCommand(
    int InstanceId,
    string Title,
    List<FormElementModel> Elements) : IRequest<Result<FormResponse>>;