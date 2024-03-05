using Domain.Models.Relational.Common;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Storage;

public interface IStorageService
{
    Task<ICollection<Media>> WriteFileAsync(ICollection<IFormFile> files, AttachmentType attachmentType);
    Task<Media?> WriteFileAsync(IFormFile file, AttachmentType attachmentType);
    Task<Media?> WriteFileAsync(MemoryStream stream2, AttachmentType attachmentType, string extension);
}
