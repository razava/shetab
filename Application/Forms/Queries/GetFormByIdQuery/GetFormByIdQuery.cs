using Application.Forms.Common;

namespace Application.Forms.Queries.GetFormByIdQuery;

public record GetFormByIdQuery(Guid Id) : IRequest<Result<FormResponse>>;


