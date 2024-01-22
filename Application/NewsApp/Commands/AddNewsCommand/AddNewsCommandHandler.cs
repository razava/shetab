using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Infrastructure.Storage;
using MediatR;

namespace Application.NewsApp.Commands.AddNewsCommand;

internal sealed class AddNewsCommandHandler(
    INewsRepository newsRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<AddNewsCommand, Result<News>>
{
    public async Task<Result<News>> Handle(AddNewsCommand request, CancellationToken cancellationToken)
    {
        var media = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (media is null)
            return AttachmentErrors.SaveImageFailed;

        var news = News.Create(request.InstanceId, request.Title, request.Description, request.Url, media, request.IsDeleted);

        newsRepository.Insert(news);
        await unitOfWork.SaveAsync();

        return news;
    }
}
