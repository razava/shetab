using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using Infrastructure.Storage;
using MediatR;

namespace Application.Polls.Commands.AnswerPollCommand;

internal class AnswerPollCommandHandler : IRequestHandler<AnswerPollCommand, bool>
{
    private readonly IPollRepository _pollRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AnswerPollCommandHandler(IPollRepository pollRepository, IUnitOfWork unitOfWork)
    {
        _pollRepository = pollRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AnswerPollCommand request, CancellationToken cancellationToken)
    {
        var poll = await _pollRepository.GetById(request.Id, request.UserId);
        if (poll is null)
            throw new Exception("Poll not found!");

        poll.Answer(request.UserId, request.ChoicesIds, request.Text);
        
        _pollRepository.Update(poll);

        await _unitOfWork.SaveAsync();

        return true;
    }
}
