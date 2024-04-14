using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.QuickAccesses.Common;
using Domain.Models.Relational;
using Infrastructure.Storage;
using Mapster;
using SharedKernel.Successes;

namespace Application.QuickAccesses.Commands.AddQuickAccess;

internal sealed class AddQuickAccessCommandHandler(
    IQuickAccessRepository quickAccessRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<AddQuickAccessCommand, Result<AdminGetQuickAccessResponse>>
{

    public async Task<Result<AdminGetQuickAccessResponse>> Handle(AddQuickAccessCommand request, CancellationToken cancellationToken)
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

        return ResultMethods.GetResult(quickAccess.Adapt<AdminGetQuickAccessResponse>(), CreationSuccess.QuickAccess);
    }
}
