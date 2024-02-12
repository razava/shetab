namespace Application.ReportNotes.Commands.DeleteReportNote;

public sealed record DeleteReportNoteCommand(
    Guid Id,
    string UserId)
    : IRequest<Result<bool>>;
