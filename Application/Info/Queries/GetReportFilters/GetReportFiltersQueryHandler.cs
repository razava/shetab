using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Common;
using Application.Info.Queries.GetInfoQuery;
using Domain.Models.Relational.Common;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.ExtensionMethods;

namespace Application.Info.Queries.GetReportFilters;

internal class GetReportFiltersQueryHandler(
    ICategoryRepository categoryRepository,
    IActorRepository actorRepository,
    IInfoService infoService,
    IUserRepository userRepository)
    : IRequestHandler<GetReportFiltersQuery, Result<ReportFiltersResponse>>
{
    public async Task<Result<ReportFiltersResponse>> Handle(GetReportFiltersQuery request, CancellationToken cancellationToken)
    {
        int instanceId = 0;
        if (request.InstanceId is not null)
            instanceId = request.InstanceId.Value;

        var priorityFilterItems = new List<FilterItem<int>>();
        foreach (var item in Enum.GetValues(typeof(Priority)))
            priorityFilterItems.Add(new FilterItem<int>(((Priority)item).GetDescription() ?? "", (int)item));

        var statusFilterItems = new List<FilterItem<int>>();
        foreach (var item in Enum.GetValues(typeof(ReportState)))
            statusFilterItems.Add(new FilterItem<int>(((ReportState)item).GetDescription() ?? "", (int)item));

        var categoryRoot = await categoryRepository.GetStaffCategories(instanceId, request.UserId, request.UserRoles);


        var regionFilterItems = new List<FilterItem<int>>();
        var regionsResult = await actorRepository.GetUserRegionsAsync(instanceId, request.UserId);
        if (regionsResult.IsSuccess)
        {
            regionFilterItems = regionsResult.Value.Select(r => new FilterItem<int>(r.RegionName, r.RegionId)).ToList();
        }
        

        var reportsToInclude = new List<FilterItem<int>>();
        foreach (var item in Enum.GetValues(typeof(ReportsToInclude)))
            reportsToInclude.Add(new FilterItem<int>(((ReportsToInclude)item).GetDescription() ?? "", (int)item));


        var satisfactionFilterItems = new List<int> { 1, 2, 3, 4, 5 }.Select(s => new FilterItem<int>(s.ToString(), s)).ToList();
        satisfactionFilterItems.Insert(0, new FilterItem<int>("بدون خشنودی سنجی", 0));


        var userIds = new List<string>();
        if (request.UserRoles.Contains(RoleNames.Manager))
        {
            userIds.AddRange(await infoService.GetUserIdsOfOrganizationalUnit(request.UserId));
        }
        if(request.UserRoles.Contains(RoleNames.Executive))
        {
            userIds.Add(request.UserId);
        }

        var executives = await userRepository.GetUsersInRole(RoleNames.Executive);
        executives = executives.Where(e => e.ShahrbinInstanceId == request.InstanceId).ToList();
        if(!userIds.IsNullOrEmpty())
        {
            executives = executives.Where(e => userIds.Contains(e.Id)).ToList();
        }
        var executiveFilterItems = executives.Select(e => 
            new FilterItem<string>($"{e.Title} ({e.FirstName} {e.LastName})".Replace(" ( )", ""), e.Id)).ToList();

        var result = new ReportFiltersResponse(
            regionFilterItems,
            categoryRoot?.Adapt<FilterCategory>(),
            statusFilterItems,
            priorityFilterItems,
            reportsToInclude,
            satisfactionFilterItems,
            executiveFilterItems);

        return result;
    }
}
