using Infrastructure.Storage;

namespace Api.Contracts;

public record UploadDto(IFormFile File, AttachmentType AttachmentType);


public record CheckUploadsDto(List<Guid> AttachmentIds, string UserId);

public record UpdateListDto(List<Guid> OldList, List<Guid> NewList, string UserId);

public record GetMediasDto(List<Guid> AttachmentIds);
