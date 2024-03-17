using Application.Common.Interfaces.Persistence;

namespace Application.Users.Commands.UpdateCategories;

internal class UpdateCategoriesCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository) : IRequestHandler<UpdateCategoriesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepository.UpdateCategoriesAsync(request.id, request.CategoryIds);
        await unitOfWork.SaveAsync();
        return true;
    }
}
