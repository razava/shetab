using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using MediatR;

namespace Application.Polls.Commands.AddPollCommand;

internal class AddPollCommandHandler : IRequestHandler<AddPollCommand, Poll>
{
    private readonly IPollRepository _pollRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork)
    {
        _pollRepository = pollRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Poll> Handle(AddPollCommand request, CancellationToken cancellationToken)
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
            request.IsActive);

        _pollRepository.Add(poll);
        await _unitOfWork.SaveAsync();

        return poll;
    }
}
