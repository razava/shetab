using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.MessageToCitizen;

internal sealed class MessageToCitizenCommandHandler : IRequestHandler<MessageToCitizenCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICommunicationService _communication;
    private readonly IActorRepository _actorRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MessageToCitizenCommandHandler(
        IUnitOfWork unitOfWork,
        IReportRepository reportRepository,
        ICommunicationService communication,
        IActorRepository actorRepository,
        IUploadRepository uploadRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _communication = communication;
        _actorRepository = actorRepository;
        _uploadRepository = uploadRepository;
    }

    public async Task<Report> Handle(MessageToCitizenCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserRoles.Contains(RoleNames.Executive))
        {
            throw new ExecutiveOnlyLimitException();
        }
        var actor = await _actorRepository.GetSingleAsync(a => a.Identifier == request.UserId);
        if (actor == null)
        {
            throw new AccessDeniedException("کاربر جاری Actor نیست.");
        }
        var report = await _reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            throw new NotFoundException("گزارش");

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await _uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.UserId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    throw new AttachmentsFailureException();
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        report.MessageToCitizen(actor.Identifier, medias, request.Message, request.Comment);
        await _unitOfWork.SaveAsync();

        return report;
    }
}
