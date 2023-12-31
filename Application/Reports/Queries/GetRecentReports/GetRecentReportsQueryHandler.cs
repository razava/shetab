using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetRecentReports;

internal sealed class GetRecentReportsQueryHandler : IRequestHandler<GetRecentReportsQuery, PagedList<Report>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;

    public GetRecentReportsQueryHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetRecentReportsQuery request, CancellationToken cancellationToken)
    {
        //var reports = await _reportRepository.GetPagedAsync(
        //    request.PagingInfo,
        //    r => r.ReportState != ReportState.NeedAcceptance
        //         && r.Visibility == Visibility.EveryOne
        //         && r.ShahrbinInstanceId == request.instanceId,
        //    false,
        //    o => o.OrderByDescending(r => r.LastStatusDateTime));

        System.Linq.Expressions.Expression<Func<Report, bool>>? filter = r =>
            r.ReportState != ReportState.NeedAcceptance
            && r.Visibility == Visibility.EveryOne
            && r.ShahrbinInstanceId == request.instanceId;

        var context = _unitOfWork.DbContext;
        var query = context.Set<Report>()
            .AsNoTracking()
            .Where(filter)
            .Include(p => p.LikedBy.Where(q => q.Id == request.UserId))
            .OrderByDescending(r => r.LastStatusDateTime);

        var reports = await PagedList<Report>.ToPagedList(
           query,
           request.PagingInfo.PageNumber,
           request.PagingInfo.PageSize);

        return reports;
    }
}
