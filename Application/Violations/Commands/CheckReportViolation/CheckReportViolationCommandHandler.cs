using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Commands.CheckReportViolation;

internal class CheckReportViolationCommandHandler(IUnitOfWork unitOfWork, IUploadRepository uploadRepository)
    : IRequestHandler<CheckReportViolationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckReportViolationCommand request, CancellationToken cancellationToken)
    {
        var report = await unitOfWork.DbContext.Set<Report>()
                .Where(r => r.Id == request.ReportId)
                .Include(r => r.Medias)
                .FirstOrDefaultAsync();

        if (report == null)
            return NotFoundErrors.Report;

        if (request.Action == ViolationCheckResult.NoAction)
        {

        }
        else if (request.Action == ViolationCheckResult.Deleted)
        {
            report.Delete();
        }
        else if (request.Action == ViolationCheckResult.Corrected)
        {
            List<Media> medias = report.Medias.ToList();
            if (request.Attachments is not null && report.Medias.Count > request.Attachments.Count)
            {
                var deletedAttachments = report.Medias.Select(m => m.Id).ToList();
                deletedAttachments.RemoveAll(request.Attachments.Contains);

                var attachments = (await uploadRepository
                    .GetAsync(u => deletedAttachments.Contains(u.Media.Id) && u.UserId == report.CitizenId))
                    .ToList() ?? new List<Upload>();

                attachments.ForEach(a => a.IsUsed = false);
                medias = medias.Where(m => request.Attachments.Contains(m.Id)).ToList();
            }
            report.Update(request.OperatorId, null, request.Comments, null, medias, null, null);
        }

        report.ViolationChecked();
        await unitOfWork.SaveAsync();


        var now = DateTime.UtcNow;
        await unitOfWork.DbContext.Set<Violation>()
                .Where(v => v.ReportId == request.ReportId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.ViolationCheckResult, request.Action)
                    .SetProperty(a => a.ViolatoinCheckDateTime, now));

        return true;
    }
}
