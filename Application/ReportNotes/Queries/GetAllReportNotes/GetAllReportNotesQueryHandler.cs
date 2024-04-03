using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;

namespace Application.ReportNotes.Queries.GetAllReportNotes;

internal class GetAllReportNotesQueryHandler(IReportNoteRepository reportNoteRepository)
    : IRequestHandler<GetAllReportNotesQuery, Result<PagedList<ReportNoteResult>>>
{
    public async Task<Result<PagedList<ReportNoteResult>>> Handle(
        GetAllReportNotesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportNoteRepository.GetAllReportNotes(
            request.UserId,
            r => ReportNoteResult.FromReportNote(r),
            request.PagingInfo);

        return result;
    }
}
