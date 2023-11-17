using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Reports.Commands.Like;

internal class LikeCommandHandler : IRequestHandler<LikeCommand, bool>
{
    private readonly IReportLikesRepository _likesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LikeCommandHandler(IReportLikesRepository likesRepository, IUnitOfWork unitOfWork)
    {
        _likesRepository = likesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        var result = await _likesRepository.Like(request.UserId, request.ReportId, request.IsLiked);
        await _unitOfWork.SaveAsync();

        return result;
    }
}
