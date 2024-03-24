using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Commands.CheckViolation;

internal class CheckViolationCommandHandler(IUnitOfWork unitOfWork, IUploadRepository uploadRepository) 
    : IRequestHandler<CheckViolationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckViolationCommand request, CancellationToken cancellationToken)
    {
        if(request.Action == ViolationCheckResult.NoAction)
        {

        }
        else
        {
            var report = await unitOfWork.DbContext.Set<Report>()
                .Where(r => r.Id == request.ReportId)
                .Include(r => r.Medias)
                .FirstOrDefaultAsync();

            if (report == null)
                return NotFoundErrors.Report;

            if(request.Action == ViolationCheckResult.Deleted)
            {
                report.Delete();
            }
            else if(request.Action == ViolationCheckResult.Corrected)
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

            await unitOfWork.SaveAsync();
        }
        

        await unitOfWork.DbContext.Set<Violation>()
            .ExecuteUpdateAsync(v => v.SetProperty(a => a.ViolationCheckResult, request.Action));

        return true;
    }
}
