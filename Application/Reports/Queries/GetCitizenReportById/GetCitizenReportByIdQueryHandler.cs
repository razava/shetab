using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetCitizenReportById;

internal sealed class GetCitizenReportByIdQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetCitizenReportByIdQuery, Result<Report>>
{
    public async Task<Result<Report>> Handle(GetCitizenReportByIdQuery request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetSingleAsync(
            r => r.Id == request.id,
            false);
        if (report is null)
            return NotFoundErrors.Report;
        if (report.CitizenId != request.UserId)
        {
            return AccessDeniedErrors.General;
        }
        return report;
    }
}