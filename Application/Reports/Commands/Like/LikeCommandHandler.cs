using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Reports.Commands.Like;

internal class LikeCommandHandler(IReportLikesRepository likesRepository, IUnitOfWork unitOfWork) : IRequestHandler<LikeCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        var result = await likesRepository.Like(request.UserId, request.ReportId, request.IsLiked);

        await unitOfWork.SaveAsync();

        return result;
    }
}
