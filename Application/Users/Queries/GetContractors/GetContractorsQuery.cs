using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.CreateContractor;

namespace Application.Users.Queries.GetContractors;

public record GetContractorsQuery(
    string ExecutiveId,
    PagingInfo PagingInfo) : IRequest<Result<PagedList<GetContractorsListResponse>>>;