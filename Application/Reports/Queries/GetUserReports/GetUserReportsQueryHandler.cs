using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;

namespace Application.Reports.Queries.GetUserReports;

internal sealed class GetUserReportsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetUserReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{
    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetUserReportsQuery request,
        CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext.Set<Report>();
        var query = context.Where(r => r.CitizenId == request.UserId);
        var query2 = query
            .OrderByDescending(r => r.LastStatusDateTime)
            .Select(r => GetCitizenReportsResponse.FromReport(r, request.UserId));

        var reports = await PagedList<GetCitizenReportsResponse>.ToPagedList(
           query2,
           request.PagingInfo.PageNumber,
           request.PagingInfo.PageSize);

        return reports;
    }
}
