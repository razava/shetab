namespace Application.Feedbacks.Commands.StoreFeedback;

public sealed record StoreFeedbackCommand(
    Guid? ReportId,
    string? UserId,
    string? Token,
    int Rating
    ) : IRequest<Result<bool>>;
