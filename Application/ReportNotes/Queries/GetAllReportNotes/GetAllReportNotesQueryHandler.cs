using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.ReportNotes.Queries.GetAllReportNotes;

internal class GetAllReportNotesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllReportNotesQuery, Result<List<ReportNoteResult>>>
{
    public async Task<Result<List<ReportNoteResult>>> Handle(
        GetAllReportNotesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<ReportNote>()
            .Where(r => r.UserId == request.UserId)
            .Select(r => ReportNoteResult.FromReportNote(r))
            .ToListAsync();

        return result;
    }
}
