using Application.Common.Interfaces.Persistence;
using Application.Feedbacks.Commands.SendFeedback;
using Domain.Models.Relational;
using Infrastructure.Storage;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Medias.Commands.AddMedia;

internal sealed class AddMediaCommandHandler : IRequestHandler<AddMediaCommand, Media>
{
    private readonly IMediaRepository _mediaRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public AddMediaCommandHandler(IUnitOfWork unitOfWork, IMediaRepository mediaRepository, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _mediaRepository = mediaRepository;
        _storageService = storageService;
    }

    public async Task<Media> Handle(AddMediaCommand request, CancellationToken cancellationToken)
    {
        var media = await _storageService.WriteFileAsync(request.File, request.AttachmentType);
        if (media == null)
        {
            throw new Exception();
        }
        _mediaRepository.Insert(media);
        await _unitOfWork.SaveAsync();

        return media;
    }
}
