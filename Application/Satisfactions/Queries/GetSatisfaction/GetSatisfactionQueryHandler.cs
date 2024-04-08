using Application.Common.Interfaces.Persistence;
using Application.Satisfactions.Commands.UpsertSatisfaction;

namespace Application.Satisfactions.Queries.GetSatisfaction;

internal sealed class GetSatisfactionQueryHandler(
    ISatisfactionRepository satisfactionRepository)
    : IRequestHandler<GetSatisfactionQuery, Result<SatisfactionResponse>>
{
    public async Task<Result<SatisfactionResponse>> Handle(GetSatisfactionQuery request, CancellationToken cancellationToken)
    {
        var result = await satisfactionRepository.GetSingleAsync(s => s.ReportId == request.ReportId);
        if (result is null)
            return new SatisfactionResponse("", "", 0, "");

        return new SatisfactionResponse(result.ActorId, result.Comments, result.Rating, result.History);
    }
}
