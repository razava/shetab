﻿using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using SharedKernel.Successes;

namespace Application.QuickAccesses.Commands.UpdateQuickAccess;

internal sealed class UpdateQuickAccessCommandHandler(
    IQuickAccessRepository quickAccessRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<UpdateQuickAccessCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateQuickAccessCommand request, CancellationToken cancellationToken)
    {
        Media? media = null;
        if (request.Image is not null)
        {
            media = await storageService.WriteFileAsync(request.Image, AttachmentType.News);
            if (media is null)
                return AttachmentErrors.SaveImageFailed;
        }


        var quickAccess = await quickAccessRepository.GetSingleAsync(q => q.Id == request.Id);
        if (quickAccess is null)
            return NotFoundErrors.QuickAccess;

        quickAccess.CategoryId = request.CategoryId ?? quickAccess.CategoryId;
        quickAccess.Title = request.Title ?? quickAccess.Title;
        quickAccess.Media = media ?? quickAccess.Media;
        quickAccess.Order = request.Order ?? quickAccess.Order;
        quickAccess.IsDeleted = request.IsDeleted ?? quickAccess.IsDeleted;

        quickAccessRepository.Update(quickAccess);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.QuickAccess);
    }
}
