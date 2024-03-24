using Domain.Models.Relational.Common;

namespace Application.Violations.Commands.CheckViolation;

public record CheckViolationCommand(
    Guid ReportId,
    string OperatorId,
    ViolationCheckResult Action,
    string? Comments,
    List<Guid>? Attachments) : IRequest<Result<bool>>;
