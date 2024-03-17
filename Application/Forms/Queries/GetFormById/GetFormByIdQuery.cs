using Application.Forms.Common;

namespace Application.Forms.Queries.GetFormById;

public record GetFormByIdQuery(Guid Id) : IRequest<Result<FormResponse>>;


