using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetReportById;

internal sealed class GetReportByIdQueryHandler(IReportRepository reportRepository, IActorRepository actorRepository) 
    : IRequestHandler<GetReportByIdQuery, Result<GetReportByIdResponse>>
{

    public async Task<Result<GetReportByIdResponse>> Handle(
        GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetByIdSelective(
            request.Id,
            r => true,
            GetReportByIdResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.Report;

        if (result.CurrentActorId is not null)
        {
            var currentActorIdentity = await actorRepository.GetActorIdentityAsync(result.CurrentActorId.Value);
            if(currentActorIdentity.IsSuccess)
                result.CurrentActor = currentActorIdentity.Value;
        }
        return result;
    }
}