using Application.Info.Common;

namespace Application.Polls.Queries.GetPollResult;

public record GetPollResultQuery(int PollId) : IRequest<Result<InfoModel>>;

