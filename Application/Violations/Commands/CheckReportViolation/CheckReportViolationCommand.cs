using Domain.Models.Relational.Common;

namespace Application.Violations.Commands.CheckReportViolation;

public record CheckReportViolationCommand(
    Guid ReportId,
    string OperatorId,
    ViolationCheckResult Action,
    string? Comments,
    List<Guid>? Attachments) : IRequest<Result<bool>>;
