using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using Mapster;

namespace Application.NewsApp.Commands.UpdateNews;

internal sealed class UpdateNewsCommandHandler(
    INewsRepository newsRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<UpdateNewsCommand, Result<GetNewsResponse>>
{

    public async Task<Result<GetNewsResponse>> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        Media? media = null;
        if (request.Image is not null)
        {
            media = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
            if (media is null)
                return AttachmentErrors.SaveImageFailed;
        }


        var news = await newsRepository.GetSingleAsync(q => q.Id == request.Id);
        if (news is null)
            return NotFoundErrors.News;

        news.Update(request.Title, request.Description, request.Url, media, request.IsDeleted);

        newsRepository.Update(news);
        await unitOfWork.SaveAsync();

        return news.Adapt<GetNewsResponse>();
    }
}
