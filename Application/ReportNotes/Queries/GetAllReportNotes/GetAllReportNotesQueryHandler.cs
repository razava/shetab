using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;
using Domain.Models.Relational.ReportAggregate;

namespace Application.ReportNotes.Queries.GetAllReportNotes;

internal class GetAllReportNotesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllReportNotesQuery, Result<PagedList<ReportNoteResult>>>
{
    public async Task<Result<PagedList<ReportNoteResult>>> Handle(
        GetAllReportNotesQuery request,
        CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<ReportNote>()
            .Where(r => r.UserId == request.UserId && r.IsDeleted == false)
            .Select(r => ReportNoteResult.FromReportNote(r));

        var result = await PagedList<ReportNoteResult>.ToPagedList(
            query,
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return result;
    }
}
