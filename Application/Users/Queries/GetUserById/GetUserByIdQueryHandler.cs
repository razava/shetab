using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserById;

internal class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetUserByIdQuery, Result<AdminGetUserDetailsResponse>>
{

    public async Task<Result<AdminGetUserDetailsResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => u.Id == request.UserId)
            .Select(AdminGetUserDetailsResponse.GetSelector())
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (user is null)
            return NotFoundErrors.User;
        return user;
    }
}
