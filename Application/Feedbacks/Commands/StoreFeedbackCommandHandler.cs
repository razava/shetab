using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Feedbacks.Commands;

internal sealed class StoreFeedbackCommandHandler : IRequestHandler<StoreFeedbackCommand, bool>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IReportRepository _reportRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StoreFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork,
        IReportRepository reportRepository)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
    }

    public async Task<bool> Handle(StoreFeedbackCommand request, CancellationToken cancellationToken)
    {
        //TODO: It's not completed yet. Decide where to put feedback, I think it's better to be implemented inside report.
        var feedback = await _feedbackRepository.GetSingleAsync(
            f => f.UserId == request.UserId && f.Token == request.Token && f.ReportId == request.ReportId);
        if ((feedback is null))
        {
            throw new ServerNotFoundException("خطایی رخ داد.", new FeedbackNotFoundException());
        }
        var report = await _reportRepository.GetSingleAsync(r => r.Id == request.ReportId);
        if (report is null)
        {
            throw new NotFoundException("گزارش");
        }
        report.UpdateFeedback(request.Rating);

        await _unitOfWork.SaveAsync();

        return true;
    }
}