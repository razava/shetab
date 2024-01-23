using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using MediatR;

namespace Application.Polls.Commands.UpdatePollCommand;

internal class UpdatePollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdatePollCommand, Result<Poll>>
{
    
    public async Task<Result<Poll>> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetById(request.Id);
        if (poll is null)
            return NotFoundErrors.Poll;


        List<PollChoice>? choices = new();
        if (request.Choices is not null)
            request.Choices.ForEach(c => choices.Add(PollChoice.Create(c.ShortTitle, c.Text, c.Order)));
        else
            choices = null;

        poll.Update(request.Title, request.PollType, request.Question, choices, request.PollState, request.isDeleted);
        pollRepository.Update(poll);

        await unitOfWork.SaveAsync();

        return poll;
    }
}
