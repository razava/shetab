using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReportById;

internal sealed class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, Report>
{
    private readonly IReportRepository _reportRepository;

    public GetReportByIdQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<Report> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        //TODO: check whether user can access to content or not
        var report = await _reportRepository.GetSingleAsync(
            r => r.Id == request.id,
            false);
        if (report is null)
            throw new NotFoundException("Report");
        return report;
    }
}