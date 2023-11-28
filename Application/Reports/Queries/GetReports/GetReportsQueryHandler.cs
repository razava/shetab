using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
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
        var actors = await _userRepository.GetActorsAsync(request.UserId);
        var actorIds = actors.Select(a => a.Id).ToList();
        System.Linq.Expressions.Expression<Func<Report, bool>>? filter;
        if (request.FromRoleId is null)
        {
            filter = r => r.ReportState == ReportState.NeedAcceptance && r.ShahrbinInstanceId == request.InstanceId;
        }
        else
        {
            filter = r => r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value) &&
                     r.LastTransition != null && r.LastTransition.From.DisplayRoleId == request.FromRoleId &&
                     r.ShahrbinInstanceId == request.InstanceId;
        }
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            filter,
            false,
            a => a.OrderBy(r => r.Sent));

        return reports;
    }
}
