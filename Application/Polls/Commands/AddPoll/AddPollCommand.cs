using Application.Polls.Common;
using Domain.Models.Relational.Common;

namespace Application.Polls.Commands.AddPoll;

public record AddPollCommand(
    int InstanceId,
    string UserId,
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceRequest> Choices,
    bool IsActive) : IRequest<Result<GetPollsResponse>>;
