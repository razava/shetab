namespace Application.Uploads.Queries.CheckUploads;

public record CheckUploadsQuery(List<Guid> AttachmentIds, string UserId) : IRequest<Result<List<Guid>>>;
