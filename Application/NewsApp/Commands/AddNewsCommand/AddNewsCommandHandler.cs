using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Infrastructure.Storage;
using MediatR;

namespace Application.NewsApp.Commands.AddNewsCommand;

internal sealed class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, News>
{
    private readonly INewsRepository _newsRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public AddNewsCommandHandler(
        INewsRepository newsRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<News> Handle(AddNewsCommand request, CancellationToken cancellationToken)
    {
        var media = await _storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (media is null)
            throw new Exception("Image not found.");

        var news = News.Create(request.Title, request.Description, request.Url, media, request.IsDeleted);

        _newsRepository.Insert(news);
        await _unitOfWork.SaveAsync();

        return news;
    }
}
