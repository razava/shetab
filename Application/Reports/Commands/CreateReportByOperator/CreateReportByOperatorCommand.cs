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
    AddressInfo Address,
    List<Guid> Attachments,
    bool IsIdentityVisible = true,
    bool IsPublic = true) : IRequest<Report>;

