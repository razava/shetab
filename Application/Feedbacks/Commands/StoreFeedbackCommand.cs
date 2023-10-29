using MediatR;

namespace Application.Feedbacks.Commands;

public sealed record StoreFeedbackCommand(
    Guid ReportId,
    string UserId,
    string Token
    ):IRequest<bool>;
