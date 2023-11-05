using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.QuickAccesses.Commands.AddQuickAccessCommand;

internal sealed class AddQuickAccessCommandHandler : IRequestHandler<AddQuickAccessCommand, QuickAccess>
{
    private readonly IQuickAccessRepository _quickAccessRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddQuickAccessCommandHandler(
        IQuickAccessRepository quickAccessRepository,
        IUnitOfWork unitOfWork,
        IMediaRepository mediaRepository)
    {
        _quickAccessRepository = quickAccessRepository;
        _unitOfWork = unitOfWork;
        _mediaRepository = mediaRepository;
    }

    public async Task<QuickAccess> Handle(AddQuickAccessCommand request, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetSingleAsync(m => m.Id == request.ImageId);
        if (media is null)
            throw new Exception("Image not found.");
        _mediaRepository.Delete(media);

        var quickAccess = new QuickAccess()
        {
            ShahrbinInstanceId = request.InstanceId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Media = media,
            Order = request.Order,
            IsDeleted = request.IsDeleted,
        };

        _quickAccessRepository.Insert(quickAccess);
        await _unitOfWork.SaveAsync();

        return quickAccess;
    }
}
