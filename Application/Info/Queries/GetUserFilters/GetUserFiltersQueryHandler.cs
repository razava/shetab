using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Common;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Info.Queries.GetUserFilters;

internal class GetUserFiltersQueryHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository)
    : IRequestHandler<GetUserFiltersQuery, Result<UserFiltersResponse>>
{
    public async Task<Result<UserFiltersResponse>> Handle(GetUserFiltersQuery request, CancellationToken cancellationToken)
    {
        var regionFilterItems = await unitOfWork.DbContext.Set<ShahrbinInstance>()
            .Where(si => si.Id == request.InstanceId)
            .SelectMany(si => si.City.Regions.Select(r => new FilterItem<int>(r.Name, r.Id)))
            .ToListAsync();

        var roles = await userRepository.GetRoles();
        var roleFilterItems = (await userRepository.GetRoles())
            .Select(role => new FilterItem<string>(role.Title, role.Name ?? ""))
            .ToList();
        roleFilterItems.RemoveAll(p => p.Value == RoleNames.PowerUser || p.Value == RoleNames.GoldenUser);

        var result = new UserFiltersResponse(regionFilterItems, roleFilterItems);

        return result;
    }
}
