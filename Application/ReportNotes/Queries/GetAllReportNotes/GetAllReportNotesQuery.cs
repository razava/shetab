using Application.ReportNotes.Common;

namespace Application.ReportNotes.Queries.GetAllReportNotes;

public sealed record GetAllReportNotesQuery(string UserId) 
    : IRequest<Result<List<ReportNoteResult>>>;

