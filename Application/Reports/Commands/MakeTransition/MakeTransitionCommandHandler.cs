using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Messages;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;

namespace Application.Reports.Commands.CreateReportByOperator;

internal sealed class MakeTransitionCommandHandler : IRequestHandler<MakeTransitionCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MakeTransitionCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
    }

    public async Task<Report> Handle(MakeTransitionCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIDAsync(request.ReportId);
        if (report == null)
            throw new Exception("Report not found");

        report.MakeTransition(
            request.TransitionId,
            request.ReasonId,
            request.Attachments,
            request.Comment,
            ActorType.Person,
            request.ActorIdentifier,
            request.ActorIds,
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
