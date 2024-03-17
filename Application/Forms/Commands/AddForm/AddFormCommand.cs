using Application.Forms.Common;

namespace Application.Forms.Commands.AddForm;

public sealed record AddFormCommand(
    int InstanceId,
    string Title,
    List<FormElementModel> Elements) : IRequest<Result<FormResponse>>;