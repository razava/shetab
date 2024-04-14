using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Polls.Common;
using Domain.Models.Relational.PollAggregate;
using Mapster;
using SharedKernel.Successes;

namespace Application.Polls.Commands.AddPoll;

internal class AddPollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<AddPollCommand, Result<GetPollsResponse>>
{

    public async Task<Result<GetPollsResponse>> Handle(AddPollCommand request, CancellationToken cancellationToken)
    {
        var choices = new List<PollChoice>();
        request.Choices.ForEach(c => choices.Add(PollChoice.Create(c.ShortTitle, c.Text, c.Order)));

        var poll = Poll.Create(
            request.InstanceId,
            request.UserId,
            request.Title,
            request.PollType,
            request.Question,
            choices,
            !request.IsActive);

        pollRepository.Add(poll);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(poll.Adapt<GetPollsResponse>(), CreationSuccess.Poll);
    }
}
