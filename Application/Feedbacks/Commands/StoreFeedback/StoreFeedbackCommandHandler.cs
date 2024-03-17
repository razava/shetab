using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Feedbacks.Commands.StoreFeedback;

internal sealed class StoreFeedbackCommandHandler(
    IFeedbackRepository feedbackRepository,
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository) : IRequestHandler<StoreFeedbackCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(StoreFeedbackCommand request, CancellationToken cancellationToken)
    {
        Feedback? feedback;

        if (request.ReportId is not null && request.UserId is not null)
        {
            feedback = await feedbackRepository.GetSingleAsync(
                f => f.UserId == request.UserId && f.ReportId == request.ReportId);
        }
        else if (request.Token is not null)
        {
            feedback = await feedbackRepository.GetSingleAsync(
                f => f.Token == request.Token);
        }
        else
        {
            return NotFoundErrors.Feedback;
        }

        if (feedback is null)
        {
            return ServerNotFoundErrors.Feedback;
        }

        var report = await reportRepository.GetSingleAsync(r => r.Id == feedback.ReportId);
        if (report is null)
        {
            return NotFoundErrors.Report;
        }
        report.UpdateFeedback(request.Rating);

        await unitOfWork.SaveAsync();

        return true;
    }
}