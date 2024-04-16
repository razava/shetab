using Application.Reports.Common;

namespace Application.Reports.Commands.MakeTransition;

public sealed record MakeTransitionCommand(
    Guid ReportId,
    int TransitionId,
    int ReasonId,
    List<Guid> Attachments,
    string Comment,
    string ActorIdentifier,
    int ToActorId,
    bool IsExecutive = false,
    bool IsContractor = false) : IRequest<Result<GetReportByIdResponse>>;

