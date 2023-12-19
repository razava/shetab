using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.Uploads.Commands.CreateUpload;

internal sealed class AddUploadCommandHandler : IRequestHandler<AddUploadCommand, Upload>
{
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public AddUploadCommandHandler(IUnitOfWork unitOfWork, IUploadRepository uploadRepository, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _uploadRepository = uploadRepository;
        _storageService = storageService;
    }

    public async Task<Upload> Handle(AddUploadCommand request, CancellationToken cancellationToken)
    {
        var media = await _storageService.WriteFileAsync(request.File, request.AttachmentType);
        if (media == null)
        {
            throw new SaveImageFailedException();
        }
        var upload = new Upload()
        {
            CreatedAt = DateTime.UtcNow,
            Media = media,
            UserId = request.UserId,
        };
        _uploadRepository.Insert(upload);
        await _unitOfWork.SaveAsync();

        return upload;
    }
}
