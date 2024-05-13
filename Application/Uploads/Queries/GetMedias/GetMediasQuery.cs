using Application.Reports.Common;

namespace Application.Uploads.Queries.GetMedias;

public record GetMediasQuery( List<Guid> AttachmentIds) : IRequest<Result<List<MediaResponse>>>;
