namespace Application.ReportNotes.Commands.UpdateReportNote;

public sealed record UpdateReportNoteCommand(
    Guid Id,
    string UserId,
    string Text)
    : IRequest<Result<bool>>;
