using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using Infrastructure.Storage;
using MediatR;

namespace Application.Polls.Commands.UpdatePollCommand;

internal class UpdatePollCommandHandler : IRequestHandler<UpdatePollCommand, Poll>
{
    private readonly IStorageService _storageService;
    private readonly IPollRepository _pollRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePollCommandHandler(IStorageService storageService, IPollRepository pollRepository, IUnitOfWork unitOfWork)
    {
        _storageService = storageService;
        _pollRepository = pollRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Poll> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await _pollRepository.GetById(request.Id);
        if (poll is null)
            throw new Exception("Poll not found!");

        List<Media>? media = null;
        if(request.Medias is not null)
        {
            media = (await _storageService.WriteFileAsync(request.Medias, AttachmentType.Poll)).ToList();
            if (media is null || media.Count != request.Medias.Count)
                throw new Exception("Attachment failed.");
        }

        List<PollChoice>? choices = new();
        if (request.Choices is not null)
            request.Choices.ForEach(c => choices.Add(PollChoice.Create(c.ShortTitle, c.Text, c.Order)));
        else
            choices = null;

        poll.Update(request.Title, request.PollType, request.Question, choices, request.PollState, media);
        _pollRepository.Update(poll);

        await _unitOfWork.SaveAsync();

        return poll;
    }
}
