using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.CreateContractor;

namespace Application.Users.Queries.GetContractors;

internal class GetContractorsQueryHandler(IUserRepository userRepository) : IRequestHandler<GetContractorsQuery, Result<PagedList<GetContractorsListResponse>>>
{

    public async Task<Result<PagedList<GetContractorsListResponse>>> Handle(GetContractorsQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetContractors(
            request.ExecutiveId,
            GetContractorsListResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
