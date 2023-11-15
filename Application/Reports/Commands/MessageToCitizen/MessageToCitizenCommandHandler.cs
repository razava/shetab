using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.MessageToCitizen;

internal sealed class MessageToCitizenCommandHandler : IRequestHandler<MessageToCitizenCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICommunicationService _communication;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActorRepository _actorRepository;

    public MessageToCitizenCommandHandler(
        IUnitOfWork unitOfWork,
        IReportRepository reportRepository,
        ICommunicationService communication,
        IActorRepository actorRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _communication = communication;
        _actorRepository = actorRepository;
    }

    public async Task<Report> Handle(MessageToCitizenCommand request, CancellationToken cancellationToken)
    {
        if (!request.UserRoles.Contains(RoleNames.Executive))
        {
            throw new Exception("User must be an executive to be able to send message to citizen.");
        }
        var actor = await _actorRepository.GetSingleAsync(a => a.Identifier == request.UserId);
        if (actor == null)
        {
            throw new Exception("Actor not found.");
        }
        var report = await _reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            throw new Exception("Report not found");

        report.MessageToCitizen(actor.Identifier, request.Attachments, request.Message, request.Comment);
        await _unitOfWork.SaveAsync();

        return report;
    }
}
