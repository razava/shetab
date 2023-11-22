using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, PagedList<Report>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;

    public GetReportsQueryHandler(IReportRepository reportRepository, IUserRepository userRepository)
    {
        _reportRepository = reportRepository;
        _userRepository = userRepository;
    }

    public async Task<PagedList<Report>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var actors = await _userRepository.GetActorsAsync(request.userId);
        var actorIds = actors.Select(a => a.Id).ToList();
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value),
            false,
            a => a.OrderBy(r => r.Sent));

        return reports;
    }
}
