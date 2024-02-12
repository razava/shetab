using Application.ReportNotes.Common;

namespace Application.ReportNotes.Commands.DeleteReportNote;

public sealed record AddReportNoteCommand(
    string UserId,
    Guid ReportId,
    string Text)
    : IRequest<ReportNoteResult>;
