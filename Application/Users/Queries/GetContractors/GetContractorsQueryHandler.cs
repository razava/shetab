using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Queries.GetContractors;

internal class GetContractorsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetContractorsQuery, Result<PagedList<ApplicationUser>>>
{

    public async Task<Result<PagedList<ApplicationUser>>> Handle(GetContractorsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var query = context.Set<ExecutiveContractor>().Where(ec => ec.ExecutiveId == request.ExecutiveId)
            .Select(ec => ec.Contractor);

        var result = await PagedList<ApplicationUser>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);
        return result;
    }
}
