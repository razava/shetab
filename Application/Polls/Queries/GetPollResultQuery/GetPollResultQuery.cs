using Application.Info.Common;

namespace Application.Polls.Queries.GetPollResultQuery;

public record GetPollResultQuery(int PollId) : IRequest<Result<InfoModel>>;

