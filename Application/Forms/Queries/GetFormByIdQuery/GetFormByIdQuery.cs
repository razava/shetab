using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Queries.GetFormByIdQuery;

public record GetFormByIdQuery(Guid Id) : IRequest<Result<Form>>;


