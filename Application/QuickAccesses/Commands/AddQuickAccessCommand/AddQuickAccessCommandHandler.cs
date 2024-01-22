using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Infrastructure.Storage;
using MediatR;

namespace Application.QuickAccesses.Commands.AddQuickAccessCommand;

internal sealed class AddQuickAccessCommandHandler(
    IQuickAccessRepository quickAccessRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<AddQuickAccessCommand, Result<QuickAccess>>
{
    
    public async Task<Result<QuickAccess>> Handle(AddQuickAccessCommand request, CancellationToken cancellationToken)
    {
        var media = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
        if (media is null)
            return AttachmentErrors.SaveImageFailed;

        var quickAccess = new QuickAccess()
        {
            ShahrbinInstanceId = request.InstanceId,
            CategoryId = request.CategoryId,
            Title = request.Title,
            Media = media,
            Order = request.Order,
            IsDeleted = request.IsDeleted,
        };

        quickAccessRepository.Insert(quickAccess);
        await unitOfWork.SaveAsync();

        return quickAccess;
    }
}
