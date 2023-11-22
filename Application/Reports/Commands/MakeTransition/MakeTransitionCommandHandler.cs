using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.CreateReportByOperator;

internal sealed class MakeTransitionCommandHandler : IRequestHandler<MakeTransitionCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MakeTransitionCommandHandler(
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

    public async Task<Report> Handle(MakeTransitionCommand request, CancellationToken cancellationToken)
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
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.ActorIdentifier))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    throw new Exception("Attachments failure.");
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

        if(report.Feedback is not null && string.IsNullOrEmpty(report.Feedback.PhoneNumber))
        {
            report.Feedback.PhoneNumber = (await _userRepository.FindAsync(report.CitizenId))?.PhoneNumber ?? "";
        }
        _reportRepository.Update(report);
        await _unitOfWork.SaveAsync();


        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        return report;
    }
}
