using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Feedbacks.Commands;

internal sealed class StoreFeedbackCommandHandler : IRequestHandler<StoreFeedbackCommand, bool>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public StoreFeedbackCommandHandler(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task<bool> Handle(StoreFeedbackCommand request, CancellationToken cancellationToken)
    {
        //TODO: It's not completed yet. Decide where to put feedback, I think it's better to be implemented inside report.
        var feedback = await _feedbackRepository.GetSingleAsync(
            f=>f.UserId==request.UserId && f.Token==request.Token && f.ReportId==request.ReportId);
        if ((feedback is null))
        {
            throw new Exception("Not found.");
        }
        return true;
    }
}