using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Medias.Commands.AddMedia;

internal sealed class GetMediaQueryHandler : IRequestHandler<GetMediaQuery, Media>
{
    private readonly IMediaRepository _mediaRepository;

    public GetMediaQueryHandler(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<Media> Handle(GetMediaQuery request, CancellationToken cancellationToken)
    {
        var media = await _mediaRepository.GetSingleAsync(m => m.Id == request.id);
        if (media == null)
        {
            throw new Exception();
        }

        return media;
    }
}
