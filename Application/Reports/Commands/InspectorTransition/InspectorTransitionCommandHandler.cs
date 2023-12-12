using Application.Common.Interfaces.Persistence;
using Application.Reports.Commands.CreateReportByOperator;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.InspectorTransition;

internal sealed class InspectorTransitionCommandHandler : IRequestHandler<InspectorTransitionCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InspectorTransitionCommandHandler(
        IUnitOfWork unitOfWork,
        IReportRepository reportRepository,
        IUserRepository userRepository,
        IUploadRepository uploadRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
        _uploadRepository = uploadRepository;
    }

    public async Task<Report> Handle(InspectorTransitionCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIDAsync(request.ReportId);
        if (report == null)
            throw new Exception("Report not found");

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await _uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.InspectorId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    throw new Exception("Attachments failure.");
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

        _reportRepository.Update(report);
        await _unitOfWork.SaveAsync();

        //TODO: Send messages and handle other things!


        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        return report;
    }
}
