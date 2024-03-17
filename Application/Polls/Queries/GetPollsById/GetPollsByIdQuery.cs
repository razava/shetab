using Application.Polls.Common;

namespace Application.Polls.Queries.GetPollsById;

public record GetPollsByIdQuery(int Id, string userId) : IRequest<Result<GetPollsResponse>>;
