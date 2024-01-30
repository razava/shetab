using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.CreateReportByOperator;

public sealed record CreateReportByOperatorCommand(
    int instanceId,
    string operatorId,
    string phoneNumber,
    string firstName,
    string lastName,
    int CategoryId,
    string Comments,
    AddressInfoRequest Address,
    List<Guid> Attachments,
    bool IsIdentityVisible = true,
    bool IsPublic = true) : IRequest<Result<Report>>;

