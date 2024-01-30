using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReportById;

internal sealed class GetReportByIdQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetReportByIdQuery, Result<Report>>
{

    public async Task<Result<Report>> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: check whether user can access to content or not
        var report = await reportRepository.GetSingleAsync(
            r => r.Id == request.id,
            false);
        if (report is null)
            return NotFoundErrors.Report;
        return report;
    }
}