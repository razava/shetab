using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.InspectorTransition;

public sealed record InspectorTransitionCommand(
    Guid ReportId,
    bool IsAccepted,
    List<Guid> Attachments,
    string Comment,
    string InspectorId,
    int ToActorId,  
    int StageId,
    Visibility? Visibility) : IRequest<Report>;
