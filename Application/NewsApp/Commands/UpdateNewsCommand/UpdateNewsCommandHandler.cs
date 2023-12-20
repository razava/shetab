using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.NewsApp.Commands.UpdateNewsCommand;

internal sealed class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, News>
{
    private readonly INewsRepository _newsRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNewsCommandHandler(
        INewsRepository newsRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<News> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        Media? media = null;
        if (request.Image is not null)
        {
            media = await _storageService.WriteFileAsync(request.Image, AttachmentType.News);
            if (media is null)
                throw new SaveImageFailedException();
        }


        var news = await _newsRepository.GetSingleAsync(q => q.Id == request.Id);
        if (news is null)
            throw new NotFoundException("News");

        news.Update(request.Title, request.Description, request.Url, media, request.IsDeleted);

        _newsRepository.Update(news);
        await _unitOfWork.SaveAsync();

        return news;
    }
}
