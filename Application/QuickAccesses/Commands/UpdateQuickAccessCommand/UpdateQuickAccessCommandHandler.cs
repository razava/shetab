using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.QuickAccesses.Commands.UpdateQuickAccessCommand;

internal sealed class UpdateQuickAccessCommandHandler : IRequestHandler<UpdateQuickAccessCommand, QuickAccess>
{
    private readonly IQuickAccessRepository _quickAccessRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuickAccessCommandHandler(
        IQuickAccessRepository quickAccessRepository,
        IUnitOfWork unitOfWork,
        IMediaRepository mediaRepository)
    {
        _quickAccessRepository = quickAccessRepository;
        _unitOfWork = unitOfWork;
        _mediaRepository = mediaRepository;
    }

    public async Task<QuickAccess> Handle(UpdateQuickAccessCommand request, CancellationToken cancellationToken)
    {
        Media? media = null;
        if(request.ImageId is not null)
        {
            media = await _mediaRepository.GetSingleAsync(m => m.Id == request.ImageId);
            if (media is null)
                throw new Exception("Image not found.");
            _mediaRepository.Delete(media);
        }
        

        var quickAccess = await _quickAccessRepository.GetSingleAsync(q => q.Id == request.Id);
        if (quickAccess is null)
            throw new Exception("Not found.");

        quickAccess.CategoryId = request.CategoryId ?? quickAccess.CategoryId;
        quickAccess.Title = request.Title ?? quickAccess.Title;
        quickAccess.Media = media ?? quickAccess.Media;
        quickAccess.Order = request.Order ?? quickAccess.Order;
        quickAccess.IsDeleted = request.IsDeleted ?? quickAccess.IsDeleted;

        _quickAccessRepository.Update(quickAccess);
        await _unitOfWork.SaveAsync();

        return quickAccess;
    }
}
