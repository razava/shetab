using Application.Common.Interfaces.Persistence;
using Application.Violations.Queries.GetReportViolations;
using Domain.Models.Relational;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Commands.ReportViolation;

internal sealed class ReportViolationCommandHandler(IViolationRepository violationRepository, IUnitOfWork unitOfWork) : IRequestHandler<ReportViolationCommand, Result<ViolationResponse>>
{

    public async Task<Result<ViolationResponse>> Handle(ReportViolationCommand request, CancellationToken cancellationToken)
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

        await unitOfWork.DbContext.Set<Report>()
            .Where(r => r.Id == request.ReportId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.ViolationCount, r => r.ViolationCount + 1)
                .SetProperty(r => r.IsViolationChecked, false));

        return violation.Adapt<ViolationResponse>();
    }
}
