using Infrastructure.Storage;

namespace Api.Contracts;

public record UploadDto(IFormFile File, AttachmentType AttachmentType);


