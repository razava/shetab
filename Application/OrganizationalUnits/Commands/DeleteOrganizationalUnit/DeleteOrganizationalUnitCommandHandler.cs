using Application.Common.Interfaces.Persistence;

namespace Application.OrganizationalUnits.Commands.DeleteOrganizationalUnit;

internal class DeleteOrganizationalUnitCommandHandler(
    IOrganizationalUnitRepository organizationalUnitRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrganizationalUnitCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {

        await organizationalUnitRepository.PhysicalDelete(request.Id);
        await unitOfWork.SaveAsync();

        return true;
    }
}
