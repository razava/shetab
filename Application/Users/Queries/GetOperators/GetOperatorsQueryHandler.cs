using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Users.Queries.GetOperators;

namespace Application.Users.Queries.GetContractors;

internal class GetOperatorsQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetOperatorsQuery, Result<List<GetSelectListResponse<string>>>>
{
    public async Task<Result<List<GetSelectListResponse<string>>>> Handle(GetOperatorsQuery request, CancellationToken cancellationToken)
    {
        var operators = await userRepository.GetUsersInRole(RoleNames.Operator);
        var result = operators.Select(o => new GetSelectListResponse<string>($"{o.Title} ({o.FirstName} {o.LastName})", o.Id)).ToList();
        return result;
    }
}
