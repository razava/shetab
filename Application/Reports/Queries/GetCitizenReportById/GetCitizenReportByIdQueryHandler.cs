using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetCitizenReportById;

internal sealed class GetCitizenReportByIdQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetCitizenReportByIdQuery, Result<GetReportByIdResponse>>
{
    public async Task<Result<GetReportByIdResponse>> Handle(
        GetCitizenReportByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetByIdSelective(
            request.Id,
            r => r.CitizenId == request.UserId,
            GetReportByIdResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.Report;

        return result;
    }
}