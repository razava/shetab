namespace Application.Uploads.Commands.UpdateUploadsList;

public record UpdateUploadsListCommand(
    List<Guid> OldList,
    List<Guid> NewList,
    string UserId) : IRequest<Result<List<Guid>>>;
