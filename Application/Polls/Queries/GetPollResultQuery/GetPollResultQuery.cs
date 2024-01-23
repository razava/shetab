using MediatR;

namespace Application.Polls.Queries.GetPollResultQuery;

public record GetPollResultQuery(int PollId) : IRequest<Result<PollResultResponce>>;
public record PollChoiceResult(string ShortTitle, double Percentage);
public record PollResultResponce(long Count, List<PollChoiceResult> PollChoicesResults);
