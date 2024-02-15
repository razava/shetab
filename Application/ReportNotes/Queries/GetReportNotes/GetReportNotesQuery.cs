using Application.ReportNotes.Common;

namespace Application.ReportNotes.Queries.GetReportNotes;

public sealed record GetReportNotesQuery(string UserId, Guid ReportId)
    : IRequest<Result<List<ReportNoteResult>>>;

