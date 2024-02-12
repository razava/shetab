using Application.ReportNotes.Common;

namespace Application.ReportNotes.Commands.AddReportNote;

public sealed record DeleteReportNoteCommand(
    string UserId,
    Guid ReportId,
    string Text)
    : IRequest<ReportNoteResult>;
