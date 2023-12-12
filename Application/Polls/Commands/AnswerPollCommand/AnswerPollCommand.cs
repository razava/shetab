using MediatR;

namespace Application.Polls.Commands.AnswerPollCommand;

public record AnswerPollCommand(
    string UserId,
    int Id,
    string? Text,
    List<int> ChoicesIds) : IRequest<bool>;
