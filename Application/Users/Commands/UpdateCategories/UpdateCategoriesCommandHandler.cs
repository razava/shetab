using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Users.Commands.UpdateCategories;

internal class UpdateCategoriesCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository) : IRequestHandler<UpdateCategoriesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepository.UpdateCategoriesAsync(request.id, request.CategoryIds);
        await unitOfWork.SaveAsync();
        return ResultMethods.GetResult(true, UpdateSuccess.OperatorCategories);
    }
}
