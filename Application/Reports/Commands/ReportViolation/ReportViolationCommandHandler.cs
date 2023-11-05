using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.ReportViolation;

internal sealed class ReportViolationCommandHandler : IRequestHandler<ReportViolationCommand, Violation>
{
    private readonly IViolationRepository _violationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReportViolationCommandHandler(IViolationRepository violationRepository, IUnitOfWork unitOfWork)
    {
        _violationRepository = violationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Violation> Handle(ReportViolationCommand request, CancellationToken cancellationToken)
    {
        var violation = new Violation()
        {
            ReportId = request.ReportId,
            UserId = request.UserId,
            ViolationTypeId = request.ViolationType,
            Description = request.Description,
        };
        _violationRepository.Insert(violation);
        await _unitOfWork.SaveAsync();
        return violation;
    }
}
