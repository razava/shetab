using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using MediatR;

namespace Application.Polls.Commands.AddPollCommand;

internal class AddPollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddPollCommand, Result<Poll>>
{
    
    public async Task<Result<Poll>> Handle(AddPollCommand request, CancellationToken cancellationToken)
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

        return poll;
    }
}
