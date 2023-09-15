using Domain.Models.Relational;

namespace Api.Services.Storage;

public interface IStorageService
{
    Task<ICollection<Media>> WriteFileAsync(ICollection<IFormFile> files, StorageService.AttachmentType attachmentType);
    Task<Media?> WriteFileAsync(IFormFile file, StorageService.AttachmentType attachmentType);
}