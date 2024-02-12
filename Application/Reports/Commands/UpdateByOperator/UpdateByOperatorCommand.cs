using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.UpdateByOperator;

public sealed record UpdateByOperatorCommand(
    Guid reportId,
    string operatorId,
    int? CategoryId,
    string? Comments,
    AddressInfoRequest? Address,
    List<Guid>? Attachments,
    Priority? Priority,
    Visibility? Visibility) : IRequest<Result<GetReportByIdResponse>>;

