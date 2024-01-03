using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.QuickAccesses.Commands.UpdateQuickAccessCommand;

internal sealed class UpdateQuickAccessCommandHandler : IRequestHandler<UpdateQuickAccessCommand, QuickAccess>
{
    private readonly IQuickAccessRepository _quickAccessRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuickAccessCommandHandler(
        IQuickAccessRepository quickAccessRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _quickAccessRepository = quickAccessRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<QuickAccess> Handle(UpdateQuickAccessCommand request, CancellationToken cancellationToken)
    {
        Media? media = null;
        if(request.Image is not null)
        {
            media = await _storageService.WriteFileAsync(request.Image, AttachmentType.News);
            if (media is null)
                throw new SaveImageFailedException();
        }
        

        var quickAccess = await _quickAccessRepository.GetSingleAsync(q => q.Id == request.Id);
        if (quickAccess is null)
            throw new NotFoundException("دسترسی سریع");

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
