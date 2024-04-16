using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using SharedKernel.Successes;

namespace Application.Reports.Commands.MakeObjection;

internal sealed class MakeObjectionCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    IUploadRepository uploadRepository) : IRequestHandler<MakeObjectionCommand, Result<Report>>
{

    public async Task<Result<Report>> Handle(MakeObjectionCommand request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIDAsync(request.ReportId);
        if (report == null)
            return NotFoundErrors.Report;

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await uploadRepository.GetAsync(
                    u => request.Attachments.Contains(u.Id) && u.UserId == request.UserId))
                    .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    return AttachmentErrors.AttachmentsFailure;
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        if(report.CitizenId == request.UserId)
        {
            report.MakeObjection(
            medias,
            request.Comment);
        }
        else if (request.UserRoles.Contains(RoleNames.Inspector))
        {
            report.MakeObjectionByInspector(
            medias,
            request.Comment,
            request.UserId);
        }
        else
        {
            return AccessDeniedErrors.General;
        }
        

        reportRepository.Update(report);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(report, OperationSuccess.MakeObjection);
    }
}
