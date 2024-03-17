using Application.Polls.Common;

namespace Application.Polls.Queries.GetPolls;

public record GetPollsQuery(int InstanceId, string UserId, bool ReturnAll = false) 
    : IRequest<Result<List<GetPollsResponse>>>;

