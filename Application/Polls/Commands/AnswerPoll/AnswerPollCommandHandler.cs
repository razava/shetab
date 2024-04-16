using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Polls.Commands.AnswerPoll;

internal class AnswerPollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork) : IRequestHandler<AnswerPollCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(AnswerPollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetById(request.Id, request.UserId);
        if (poll is null)
            return NotFoundErrors.Poll;

        poll.Answer(request.UserId, request.ChoicesIds, request.Text);

        pollRepository.Update(poll);

        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, RegistrationSuccess.PollResponse);
    }
}
