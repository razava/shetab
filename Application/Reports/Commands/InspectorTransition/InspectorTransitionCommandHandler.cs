using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.InspectorTransition;

internal sealed class InspectorTransitionCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    IUserRepository userRepository,
    IUploadRepository uploadRepository) : IRequestHandler<InspectorTransitionCommand, Result<Report>>
{
    
    public async Task<Result<Report>> Handle(InspectorTransitionCommand request, CancellationToken cancellationToken)
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
                attachments = (await uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.InspectorId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    return AttachmentErrors.AttachmentsFailure;
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        report.MoveToStage(
            request.IsAccepted,
            request.StageId,
            request.ToActorId,
            request.Comment,
            medias,
            request.InspectorId,
            request.Visibility);

        reportRepository.Update(report);
        await unitOfWork.SaveAsync();

        //TODO: Send messages and handle other things!


        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        return report;
    }
}
