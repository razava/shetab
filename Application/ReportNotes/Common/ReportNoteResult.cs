using Domain.Models.Relational.ReportAggregate;

namespace Application.ReportNotes.Common;

public record ReportNoteResult(
    Guid Id,
    Guid ReportId,
    string Text,
    DateTime Created,
    DateTime Updated)
{
    public static ReportNoteResult FromReportNote(ReportNote note)
    {
        return new ReportNoteResult(note.Id, note.ReportId, note.Text, note.Created, note.Updated);
    }
}
