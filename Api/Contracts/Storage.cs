using static Api.Services.Storage.StorageService;

namespace Api.Contracts;

public record UploadDto(IFormFile File, AttachmentType AttachmentType);