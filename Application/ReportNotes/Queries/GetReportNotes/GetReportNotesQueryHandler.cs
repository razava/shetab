using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;

namespace Application.ReportNotes.Queries.GetReportNotes;

internal class GetReportNotesQueryHandler(IReportNoteRepository reportNoteRepository)
    : IRequestHandler<GetReportNotesQuery, Result<List<ReportNoteResult>>>
{
    public async Task<Result<List<ReportNoteResult>>> Handle(
        GetReportNotesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportNoteRepository.GetReportNotes(
            request.ReportId,
            request.UserId,
            r => ReportNoteResult.FromReportNote(r));

        return result;
    }
}
