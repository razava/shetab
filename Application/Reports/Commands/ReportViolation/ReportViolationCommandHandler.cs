using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.ReportViolation;

internal sealed class ReportViolationCommandHandler(IViolationRepository violationRepository, IUnitOfWork unitOfWork) : IRequestHandler<ReportViolationCommand, Result<Violation>>
{
    
    public async Task<Result<Violation>> Handle(ReportViolationCommand request, CancellationToken cancellationToken)
    {
        var violation = new Violation()
        {
            ShahrbinInstanceId = request.InstanceId,
            ReportId = request.ReportId,
            UserId = request.UserId,
            ViolationTypeId = request.ViolationTypeId,
            Description = request.Description,
        };
        violationRepository.Insert(violation);
        await unitOfWork.SaveAsync();
        return violation;
    }
}
