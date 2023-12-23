using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using MediatR;

namespace Application.Polls.Commands.UpdatePollCommand;

internal class UpdatePollCommandHandler : IRequestHandler<UpdatePollCommand, Poll>
{
    private readonly IPollRepository _pollRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork)
    {
        _pollRepository = pollRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Poll> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await _pollRepository.GetById(request.Id);
        if (poll is null)
            throw new NotFoundException("Poll");


        List<PollChoice>? choices = new();
        if (request.Choices is not null)
            request.Choices.ForEach(c => choices.Add(PollChoice.Create(c.ShortTitle, c.Text, c.Order)));
        else
            choices = null;

        poll.Update(request.Title, request.PollType, request.Question, choices, request.PollState, request.isDeleted);
        _pollRepository.Update(poll);

        await _unitOfWork.SaveAsync();

        return poll;
    }
}
