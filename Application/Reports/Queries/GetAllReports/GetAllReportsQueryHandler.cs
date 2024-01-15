using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetAllReports;

internal sealed class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, PagedList<Report>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;

    public GetAllReportsQueryHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        //TODO: Implement appropriate filters
        //var reports = await _reportRepository.GetPagedAsync(request.PagingInfo,
        //    r => r.ShahrbinInstanceId == request.instanceId
        //         && r.TransitionLogs.Any(tl => tl.ActorIdentifier == request.UserId),
        //    false,
        //    a => a.OrderBy(r => r.Sent));

        System.Linq.Expressions.Expression<Func<Report, bool>>? handleFilter = r =>
            r.ShahrbinInstanceId == request.instanceId
            && r.TransitionLogs.Any(tl => tl.ActorIdentifier == request.UserId);

        System.Linq.Expressions.Expression<Func<Report, bool>>? inputFilter = r =>
        ((request.FilterModel == null) ||
        (request.FilterModel.SentFromDate == null || r.Sent >= request.FilterModel.SentFromDate)
        && (request.FilterModel.SentToDate == null || r.Sent <= request.FilterModel.SentToDate)
        && (request.FilterModel.CategoryIds == null || request.FilterModel.CategoryIds.Contains(r.CategoryId))
        && (request.FilterModel.RegionIds == null || request.FilterModel.RegionIds.Contains((int)r.Address.RegionId!))
        && (request.FilterModel.CurrentStates == null || request.FilterModel.CurrentStates.Contains(r.ReportState))
        && (request.FilterModel.Query == null || r.TrackingNumber.Contains(request.FilterModel.Query))
        && (request.FilterModel.PhoneNumber == null ||
        (r.Citizen.PhoneNumber.Contains(request.FilterModel.PhoneNumber)) ||
        (r.Citizen.PhoneNumber2.Contains(request.FilterModel.PhoneNumber))));

        var context = _unitOfWork.DbContext;
        var query = context.Set<Report>()
            .AsNoTracking()
            .Where(handleFilter)
            .Where(inputFilter);

        var reports = await PagedList<Report>.ToPagedList(
            query.OrderBy(r => r.Sent),   //todo : ....... shouldn't be OrderByDescending ????
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);
        
        return reports;
    }
}
