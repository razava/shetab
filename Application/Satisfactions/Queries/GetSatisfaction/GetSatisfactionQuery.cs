using Application.Satisfactions.Commands.UpsertSatisfaction;

namespace Application.Satisfactions.Queries.GetSatisfaction;

public record GetSatisfactionQuery(Guid ReportId) : IRequest<Result<SatisfactionResponse>>;
