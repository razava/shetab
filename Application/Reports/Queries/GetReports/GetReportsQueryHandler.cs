using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, PagedList<Report>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportRepository _reportRepository;
    private readonly IUserRepository _userRepository;

    public GetReportsQueryHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _userRepository = userRepository;
    }

    public async Task<PagedList<Report>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var actors = await _userRepository.GetActorsAsync(request.UserId);
        var actorIds = actors.Select(a => a.Id).ToList();
        System.Linq.Expressions.Expression<Func<Report, bool>>? filter;
        if (request.FromRoleId is null && request.Roles.Contains(RoleNames.Operator))
        {
            filter = r => r.ReportState == ReportState.NeedAcceptance && r.ShahrbinInstanceId == request.InstanceId;
        }
        else
        {
            filter = r => r.CurrentActorId != null && actorIds.Contains(r.CurrentActorId.Value) &&
                     r.LastTransition != null && r.LastTransition.From.DisplayRoleId == request.FromRoleId &&
                     r.ShahrbinInstanceId == request.InstanceId;
        }

        System.Linq.Expressions.Expression<Func<Report, bool>>? inputFilters = r =>
        ((request.FilterGetReports == null) ||
        (request.FilterGetReports.SentFromDate == null || r.Sent >= request.FilterGetReports.SentFromDate)
        && (request.FilterGetReports.SentToDate == null || r.Sent <= request.FilterGetReports.SentToDate)
        && (request.FilterGetReports.CurrentStates == null || request.FilterGetReports.CurrentStates.Contains(r.ReportState)) 
        && (request.FilterGetReports.Query == null || r.TrackingNumber.Contains(request.FilterGetReports.Query))
        && (request.FilterGetReports.PhoneNumber == null ||
        (r.Citizen.PhoneNumber.Contains(request.FilterGetReports.PhoneNumber)) ||
        (r.Citizen.PhoneNumber2.Contains(request.FilterGetReports.PhoneNumber)) ));


        //var reports = await _reportRepository.GetPagedAsync(
        //    request.PagingInfo,
        //    filter,
        //    false,
        //    a => a.OrderBy(r => r.Sent));

        var context = _unitOfWork.DbContext;
        var query = context.Set<Report>()
            .AsNoTracking()
            .Where(filter)
            .Where(inputFilters);

        var reports = await PagedList<Report>.ToPagedList(
            query.OrderBy(r => r.Sent),
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return reports;
    }
}
