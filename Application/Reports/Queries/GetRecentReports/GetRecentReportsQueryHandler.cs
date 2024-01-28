using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetRecentReports;

internal sealed class GetRecentReportsQueryHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository) : IRequestHandler<GetRecentReportsQuery, Result<PagedList<Report>>>
{
    
    public async Task<Result<PagedList<Report>>> Handle(GetRecentReportsQuery request, CancellationToken cancellationToken)
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

        var context = unitOfWork.DbContext;
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
