using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;
using SharedKernel.Successes;

namespace Application.Reports.Commands.MessageToCitizen;

internal sealed class MessageToCitizenCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    ICommunicationService communication,
    IActorRepository actorRepository,
    IUploadRepository uploadRepository) : IRequestHandler<MessageToCitizenCommand, Result<GetReportByIdResponse>>
{
    public async Task<Result<GetReportByIdResponse>> Handle(MessageToCitizenCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserRoles.Contains(RoleNames.Executive))
        {
            return OperationErrors.ExecutiveOnlyLimit;
        }
        var actor = await actorRepository.GetSingleAsync(a => a.Identifier == request.UserId);
        if (actor == null)
        {
            //return AccessDeniedErrors.Actor;
            return AccessDeniedErrors.General;
        }
        var report = await reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            return NotFoundErrors.Report;

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.UserId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    return AttachmentErrors.AttachmentsFailure;
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        report.MessageToCitizen(actor.Identifier, medias, request.Comment);
        await unitOfWork.SaveAsync();

        var result = new Result<GetReportByIdResponse>()
            .WithValue(GetReportByIdResponse.FromReport(report))
            .WithSuccess(SuccessOperation.MessageToCitizen);

        return result;
    }
}
