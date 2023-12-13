using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.CreateReportByCitizen;

public sealed record CreateReportByCitizenCommand(
    int instanceId,
    string citizenId,
    string phoneNumber,
    int CategoryId,
    string Comments,
    AddressInfoRequest Address,
    List<Guid> Attachments,
    bool IsIdentityVisible = true,
    bool IsPublic = true) : IRequest<Report>;

