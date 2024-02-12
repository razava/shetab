using Application.ReportNotes.Common;

namespace Application.ReportNotes.Commands.AddReportNote;

public sealed record AddReportNoteCommand(
    string UserId,
    Guid ReportId,
    string Text)
    : IRequest<Result<ReportNoteResult>>;
