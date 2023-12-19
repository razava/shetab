using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetCitizenReportById;

internal sealed class GetCitizenReportByIdQueryHandler : IRequestHandler<GetCitizenReportByIdQuery, Report>
{
    private readonly IReportRepository _reportRepository;

    public GetCitizenReportByIdQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<Report> Handle(GetCitizenReportByIdQuery request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetSingleAsync(
            r => r.Id == request.id,
            false);
        if (report is null)
            throw new NotFoundException("Report");
        if (report.CitizenId != request.UserId)
        {
            throw new AccessDeniedException();
        }
        return report;
    }
}