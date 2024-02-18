using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.ReportNotes.Queries.GetReportNotes;

internal class GetReportNotesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetReportNotesQuery, Result<List<ReportNoteResult>>>
{
    public async Task<Result<List<ReportNoteResult>>> Handle(
        GetReportNotesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<ReportNote>()
            .Where(r => r.ReportId == request.ReportId && 
                        r.UserId == request.UserId && 
                        r.IsDeleted == false)
            .Select(r => ReportNoteResult.FromReportNote(r))
            .ToListAsync();

        return result;
    }
}
