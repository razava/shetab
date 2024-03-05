using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;

namespace Application.Uploads.Commands.CreateUpload;

internal sealed class AddUploadCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadRepository uploadRepository,
    IStorageService storageService) 
    : IRequestHandler<AddUploadCommand, Result<Upload>>
{
    
    public async Task<Result<Upload>> Handle(AddUploadCommand request, CancellationToken cancellationToken)
    {
        var media = await storageService.WriteFileAsync(request.File, request.AttachmentType);
        if (media == null)
            return AttachmentErrors.SaveImageFailed;
        
        var upload = new Upload()
        {
            CreatedAt = DateTime.UtcNow,
            Media = media,
            UserId = request.UserId,
        };
        uploadRepository.Insert(upload);
        await unitOfWork.SaveAsync();

        return upload;
    }
}
