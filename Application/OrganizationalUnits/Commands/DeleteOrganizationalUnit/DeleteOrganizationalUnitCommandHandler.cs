using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.OrganizationalUnits.Commands.DeleteOrganizationalUnit;

internal class DeleteOrganizationalUnitCommandHandler(
    IOrganizationalUnitRepository organizationalUnitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrganizationalUnitCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {

        await organizationalUnitRepository.PhysicalDelete(request.Id);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, DeleteSuccess.OrganizationalUnit);
    }
}
