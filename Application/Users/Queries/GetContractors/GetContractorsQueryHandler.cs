using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUsers;

internal class GetContractorsQueryHandler : IRequestHandler<GetContractorsQuery, PagedList<ApplicationUser>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetContractorsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<ApplicationUser>> Handle(GetContractorsQuery request, CancellationToken cancellationToken)
    {
        var context = _unitOfWork.DbContext;
        var query = context.Set<ExecutiveContractor>().Where(ec => ec.ExecutiveId == request.ExecutiveId)
            .Select(ec => ec.Contractor);

        var result = await PagedList<ApplicationUser>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);
        return result;
    }
}
