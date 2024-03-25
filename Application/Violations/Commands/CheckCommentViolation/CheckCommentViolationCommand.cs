using Domain.Models.Relational.Common;

namespace Application.Violations.Commands.CheckCommentViolation;

public record CheckCommentViolationCommand(
    Guid CommentId,
    string OperatorId,
    ViolationCheckResult Action,
    string? Text) : IRequest<Result<bool>>;
