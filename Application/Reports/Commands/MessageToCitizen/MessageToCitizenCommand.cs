using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.MessageToCitizen;

public sealed record MessageToCitizenCommand(
    Guid reportId,
    string UserId,
    List<string> UserRoles,
    List<Guid> Attachments,
    string Comment) : IRequest<Result<GetReportByIdResponse>>; //TODO: Fix the return typd

