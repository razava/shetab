using MediatR;

namespace Application.Feedbacks.Commands.SendFeedback;

public sealed record SendFeedbackCommand(
    int instanceId,
    Guid reportId,
    string citizenId,
    DateTime now) : IRequest<bool>;

