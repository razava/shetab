using Application.Common.Interfaces.Persistence;
using Application.Info.Common;

namespace Application.Polls.Queries.GetPollResult;

internal class GetPollResultQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetPollResultQuery, Result<InfoModel>>
{

    public async Task<Result<InfoModel>> Handle(GetPollResultQuery request, CancellationToken cancellationToken)
    {
        var result = await pollRepository.GetPollResult(request.PollId);
        return result;
    }
}