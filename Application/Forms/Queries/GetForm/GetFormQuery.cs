using Application.Forms.Common;

namespace Application.Forms.Queries.GetForm;

public record GetFormQuery(int InstanceId)
    : IRequest<Result<List<FormListItemResponse>>>;
