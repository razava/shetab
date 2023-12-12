using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using Infrastructure.Storage;
using MediatR;

namespace Application.Polls.Commands.AddPollCommand;

internal class AddPollCommandHandler : IRequestHandler<AddPollCommand, Poll>
{
    private readonly IStorageService _storageService;
    private readonly IPollRepository _pollRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPollCommandHandler(IStorageService storageService, IPollRepository pollRepository, IUnitOfWork unitOfWork)
    {
        _storageService = storageService;
        _pollRepository = pollRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Poll> Handle(AddPollCommand request, CancellationToken cancellationToken)
    {
        var media = await _storageService.WriteFileAsync(request.Medias, AttachmentType.Poll);
        if (media is null || media.Count != request.Medias.Count)
            throw new Exception("Attachment failed.");
        var choices = new List<PollChoice>();
        request.Choices.ForEach(c => choices.Add(PollChoice.Create(c.ShortTitle, c.Text, c.Order)));

        var poll = Poll.Create(
            request.InstanceId,
            request.UserId,
            request.Title,
            request.PollType,
            request.Question,
            choices,
            request.IsActive,
            media.ToList());

        _pollRepository.Add(poll);
        await _unitOfWork.SaveAsync();

        return poll;
    }
}
