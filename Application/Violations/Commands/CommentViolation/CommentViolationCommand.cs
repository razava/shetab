using Domain.Models.Relational;

namespace Application.Violations.Commands.CommentViolation;

public sealed record CommentViolationCommand(
    int InstanceId,
    Guid CommentId,
    string UserId,
    int ViolationTypeId,
    string Description) : IRequest<Result<Violation>>;

