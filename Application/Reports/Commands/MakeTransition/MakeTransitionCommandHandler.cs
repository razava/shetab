﻿using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational.Common;
using SharedKernel.Successes;

namespace Application.Reports.Commands.MakeTransition;

internal sealed class MakeTransitionCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    IUserRepository userRepository,
    IUploadRepository uploadRepository) : IRequestHandler<MakeTransitionCommand, Result<GetReportByIdResponse>>
{
    
    public async Task<Result<GetReportByIdResponse>> Handle(MakeTransitionCommand request, CancellationToken cancellationToken)
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
                    u => request.Attachments.Contains(u.Id) && u.UserId == request.ActorIdentifier))
                    .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    return AttachmentErrors.AttachmentsFailure;
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        report.MakeTransition(
            request.TransitionId,
            request.ReasonId,
            medias,
            request.Comment,
            ActorType.Person,
            request.ActorIdentifier,
            request.ToActorId,
            request.IsExecutive,
            request.IsContractor);

        reportRepository.Update(report);
        await unitOfWork.SaveAsync();


        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        return ResultMethods.GetResult(GetReportByIdResponse.FromReport(report), OperationSuccess.MakeTransition);
    }
}
