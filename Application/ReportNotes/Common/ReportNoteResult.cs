namespace Application.ReportNotes.Common;

public record ReportNoteResult(
    Guid ReportId,
    string Text,
    DateTime Created,
    DateTime Updated);
