using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.OrganizationalUnits.Commands.DeleteOrganizationalUnitCommand;

internal class DeleteOrganizationalUnitCommandHandler : IRequestHandler<DeleteOrganizationalUnitCommand, bool>
{
    private readonly IOrganizationalUnitRepository _organizationalUnitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationalUnitCommandHandler(
        IOrganizationalUnitRepository organizationalUnitRepository,
        IUnitOfWork unitOfWork)
    {
        _organizationalUnitRepository = organizationalUnitRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<bool> Handle(DeleteOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        
        await _organizationalUnitRepository.PhysicalDelete(request.Id);
        await _unitOfWork.SaveAsync();

        return true;
    }
}
