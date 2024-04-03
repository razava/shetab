using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.QuickAccesses.Queries.GetCitizenQuickAccesses;

internal class GetCitizenQuickAccessesQueryHandler(IUserRepository userRepository, IQuickAccessRepository quickAccessRepository) 
    : IRequestHandler<GetCitizenQuickAccessesQuery, Result<List<CitizenGetQuickAccessResponse>>>
{

    public async Task<Result<List<CitizenGetQuickAccessResponse>>> Handle(GetCitizenQuickAccessesQuery request, CancellationToken cancellationToken)
    {
        var roleIds = (await userRepository.GetRoles())
            .Where(r => request.Roles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToList();

        Expression<Func<QuickAccess, bool>> filter = q =>
            roleIds.Contains(q.Category.RoleId) &&
                (request.FilterModel == null || request.FilterModel.Query == null ||
                    q.Title.Contains(request.FilterModel.Query));

        var result = await quickAccessRepository.GetAll(
            request.InstanceId, CitizenGetQuickAccessResponse.GetSelector(), filter, false);

        return result;
    }
}
