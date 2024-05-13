using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Mapster;

namespace Application.Uploads.Queries.GetMedias;

internal sealed class GetMediasQueryHandler(
    IUploadRepository uploadRepository) : IRequestHandler<GetMediasQuery, Result<List<MediaResponse>>>
{
    public async Task<Result<List<MediaResponse>>> Handle(GetMediasQuery request, CancellationToken cancellationToken)
    {
        var uploads = await uploadRepository.GetAsync(u => request.AttachmentIds.Contains(u.Id));
        var result = uploads.Select(u => u.Media).ToList();

        if (result.Count != request.AttachmentIds.Count)
            return AttachmentErrors.AttachmentsFailure;

        return result.Adapt<List<MediaResponse>>();
    }
}
