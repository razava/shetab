using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.MessageToCitizen;

internal sealed class MessageToCitizenCommandHandler : IRequestHandler<MessageToCitizenCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICommunicationService _communication;
    private readonly IUnitOfWork _unitOfWork;

    public MessageToCitizenCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, ICommunicationService communication)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _communication = communication;
    }

    public async Task<Report> Handle(MessageToCitizenCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            throw new Exception("Report not found");

        report.MessageToCitizen(request.ActorIdentifier, request.ActorType, request.Attachments, request.Message, request.Comment);
        await _unitOfWork.SaveAsync();

        return report;
    }
}
