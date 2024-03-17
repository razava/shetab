namespace Application.Polls.Commands.AnswerPoll;

public record AnswerPollCommand(
    string UserId,
    int Id,
    string? Text,
    List<int> ChoicesIds) : IRequest<Result<bool>>;
