using Application.Reports.Common;
using Domain.Models.Relational.Common;

namespace Application.Reports.Commands.AcceptByOperator;

public sealed record AcceptByOperatorCommand(
    Guid reportId,
    string operatorId,
    int? CategoryId,
    string? Comments,
    AddressInfoRequest? Address,
    List<Guid>? Attachments,
    Priority? Priority,
    Visibility? Visibility) : IRequest<Result<bool>>;

