using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;

namespace Application.ReportNotes.Queries.GetAllReportNotes;

public sealed record GetAllReportNotesQuery(
    PagingInfo PagingInfo,
    string UserId) 
    : IRequest<Result<PagedList<ReportNoteResult>>>;

