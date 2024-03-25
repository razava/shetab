using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.CreateContractor;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetContractors;

internal class GetContractorsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetContractorsQuery, Result<PagedList<GetContractorsListResponse>>>
{

    public async Task<Result<PagedList<GetContractorsListResponse>>> Handle(GetContractorsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var query = context.Set<ExecutiveContractor>().Where(ec => ec.ExecutiveId == request.ExecutiveId)
            .AsNoTracking()
            .Select(ec => ec.Contractor)
            .Select(GetContractorsListResponse.GetSelector());

        var result = await PagedList<GetContractorsListResponse>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);
        return result;
    }
}
