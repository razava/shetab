using Application.Reports.Common;

namespace Application.Uploads.Queries.GetMediaById;

public record GetMediaByIdQuery(Guid Id) : IRequest<Result<MediaResponse>>;