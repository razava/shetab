using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Feedbacks.Commands;

internal sealed class StoreFeedbackCommandHandler(
    IFeedbackRepository feedbackRepository,
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository) : IRequestHandler<StoreFeedbackCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(StoreFeedbackCommand request, CancellationToken cancellationToken)
    {
        //TODO: It's not completed yet. Decide where to put feedback, I think it's better to be implemented inside report.
        var feedback = await feedbackRepository.GetSingleAsync(
            f => f.UserId == request.UserId && f.Token == request.Token && f.ReportId == request.ReportId);
        if ((feedback is null))
        {
            return ServerNotFoundErrors.Feedback;
        }
        var report = await reportRepository.GetSingleAsync(r => r.Id == request.ReportId);
        if (report is null)
        {
            return NotFoundErrors.Report;
        }
        report.UpdateFeedback(request.Rating);

        await unitOfWork.SaveAsync();

        return true;
    }
}