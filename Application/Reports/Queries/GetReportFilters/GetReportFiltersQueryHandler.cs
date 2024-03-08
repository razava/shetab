using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Application.Info.Queries.GetInfoQuery;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using SharedKernel.ExtensionMethods;

namespace Application.Reports.Queries.GetReportFilters;

internal class GetReportFiltersQueryHandler(ICategoryRepository categoryRepository, IActorRepository actorRepository)
    : IRequestHandler<GetReportFiltersQuery, Result<ReportFiltersResponse>>
{
    public async Task<Result<ReportFiltersResponse>> Handle(GetReportFiltersQuery request, CancellationToken cancellationToken)
    {
        int instanceId = 0;
        if (request.InstanceId is not null)
            instanceId = request.InstanceId.Value;

        var priorityFilterItems = new List<FilterItem>();
        foreach (var item in Enum.GetValues(typeof(Priority)))
            priorityFilterItems.Add(new FilterItem(((Priority)item).GetDescription() ?? "", (int)item));

        var statusFilterItems = new List<FilterItem>();
        foreach (var item in Enum.GetValues(typeof(ReportState)))
            statusFilterItems.Add(new FilterItem(((ReportState)item).GetDescription() ?? "", (int)item));

        var categoryRoot = (await categoryRepository.GetStaffCategories(instanceId, request.UserId, request.UserRoles));


        var regions = await actorRepository.GetUserRegionsAsync(instanceId, request.UserId);
        var regionFilterItems = regions.Select(r => new FilterItem(r.RegionName, r.RegionId)).ToList();


        var reportsToInclude = new List<FilterItem>();
        foreach (var item in Enum.GetValues(typeof(ReportsToInclude)))
            reportsToInclude.Add(new FilterItem(((ReportsToInclude)item).GetDescription() ?? "", (int)item));


        var satisfactionFilterItems = new List<int> { 1, 2, 3, 4, 5 }.Select(s => new FilterItem(s.ToString(), s)).ToList();

        var result = new ReportFiltersResponse(
            regionFilterItems,
            categoryRoot.Adapt<FilterCategory>(),
            statusFilterItems,
            priorityFilterItems,
            reportsToInclude,
            satisfactionFilterItems,
            satisfactionFilterItems);

        return result;
    }
}
