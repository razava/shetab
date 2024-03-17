using Application.Common.Interfaces.Persistence;

namespace Application.Users.Queries.GetUserCategories;

internal class GetUserCategoriesQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserCategoriesQuery, Result<List<int>>>
{
    public async Task<Result<List<int>>> Handle(GetUserCategoriesQuery request, CancellationToken cancellationToken)
    {
        //var user = await userRepository.GetSingleAsync(u => u.Id == request.UserId, false, "Categories");
        //if (user == null)
        //    return NotFoundErrors.User;
        //return user.Categories;

        var result = await userRepository.GetUserCategoriesAsync(request.UserId);

        return result;

    }
}

