using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Polls.Queries.GetPollsQuery;

public record GetPollsQuery(int InstanceId, string UserId, bool ReturnAll = false) : IRequest<List<GetPollsResponse>>;
public record GetPollsResponse(
    int Id,
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceResponse> Choices,
    PollState PollState,
    PollAnswerResponse? Answer,
    bool IsDeleted);
public record PollChoiceResponse(int Id, string ShortTitle, string Text, int Order);
public record PollAnswerResponse(List<int>? Choices, string? Text);
