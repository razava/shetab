using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.AcceptByOperator;

public sealed record AcceptByOperatorCommand(
    Guid reportId,
    string operatorId,
    int? CategoryId,
    string? Comments,
    AddressInfo? Address,
    List<Guid>? Attachments,
    bool? IsPublic = true) : IRequest<Report>;

