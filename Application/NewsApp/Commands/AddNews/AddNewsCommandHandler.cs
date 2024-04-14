using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;
using Domain.Models.Relational;
using Infrastructure.Storage;
using Mapster;
using SharedKernel.Successes;

namespace Application.NewsApp.Commands.AddNews;

internal sealed class AddNewsCommandHandler(
    INewsRepository newsRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<AddNewsCommand, Result<GetNewsResponse>>
{
    public async Task<Result<GetNewsResponse>> Handle(AddNewsCommand request, CancellationToken cancellationToken)
    {
        var media = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (media is null)
            return AttachmentErrors.SaveImageFailed;

        var news = News.Create(request.InstanceId, request.Title, request.Description, request.Url, media, request.IsDeleted);

        newsRepository.Insert(news);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(news.Adapt<GetNewsResponse>(), CreationSuccess.News);
    }
}
