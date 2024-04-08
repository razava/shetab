namespace Application.Satisfactions.Commands.UpsertSatisfaction;

public record UpsertSatisfactionCommand(Guid ReportId, string UserId, string Comment, int Rating)
    : IRequest<Result<SatisfactionResponse>>;

public record SatisfactionResponse(string UserId, string Comment, int Rating, string History);