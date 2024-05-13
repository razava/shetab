using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Mapster;

namespace Application.Uploads.Queries.GetMediaById;

internal sealed class GetMediaByIdQueryHandler(
    IUploadRepository uploadRepository) : IRequestHandler<GetMediaByIdQuery, Result<MediaResponse>>
{
    public async Task<Result<MediaResponse>> Handle(GetMediaByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await uploadRepository.GetSingleAsync(
            u => u.Id == request.Id, false);

        if (result == null)
            return NotFoundErrors.Media;

        return result.Media.Adapt<MediaResponse>();

    }
}
