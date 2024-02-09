using Application.Forms.Common;

namespace Application.Forms.Queries.GetFormQuery;

public record GetFormQuery(int InstanceId) 
    : IRequest<Result<List<FormListItemResponse>>>;
