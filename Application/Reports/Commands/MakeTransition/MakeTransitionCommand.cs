using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.CreateReportByOperator;

public sealed record MakeTransitionCommand(
    Guid ReportId,
    int TransitionId,
    int ReasonId,
    List<Guid> Attachments,
    string Comment,
    string ActorIdentifier,
    List<int> ActorIds,
    bool IsExecutive = false,
    bool IsContractor = false) : IRequest<Report>;

