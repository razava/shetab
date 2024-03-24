using Domain.Models.Relational;

namespace Application.Violations.Commands.ReportViolation;

public sealed record ReportViolationCommand(
    int InstanceId,
    Guid ReportId,
    string UserId,
    int ViolationTypeId,
    string Description) : IRequest<Result<Violation>>;

