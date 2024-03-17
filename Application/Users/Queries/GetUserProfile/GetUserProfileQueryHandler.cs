using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserProfileQuery, Result<GetProfileResponse>>
{

    public async Task<Result<GetProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<ApplicationUser>()
            .Where(u => u.Id == request.UserId)
            .Select(GetProfileResponse.GetSelector())
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFoundErrors.User;
        }

        return result;
    }
}
