using Application.ReportNotes.Common;

namespace Application.ReportNotes.Commands.UpdateReportNote;

public sealed record UpdateReportNoteCommand(
    string UserId,
    Guid ReportId,
    string Text)
    : IRequest<ReportNoteResult>;
