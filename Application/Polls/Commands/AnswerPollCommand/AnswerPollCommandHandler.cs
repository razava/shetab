using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using Infrastructure.Storage;
using MediatR;

namespace Application.Polls.Commands.AnswerPollCommand;

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

        return true;
    }
}
