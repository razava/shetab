using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Commands.CommentViolation;

internal sealed class CommentViolationCommandHandler(IViolationRepository violationRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CommentViolationCommand, Result<Violation>>
{

    public async Task<Result<Violation>> Handle(CommentViolationCommand request, CancellationToken cancellationToken)
    {
        var violation = new Violation()
        {
            ShahrbinInstanceId = request.InstanceId,
            CommentId = request.CommentId,
            UserId = request.UserId,
            ViolationTypeId = request.ViolationTypeId,
            Description = request.Description,
        };
        violationRepository.Insert(violation);
        await unitOfWork.SaveAsync();

        await unitOfWork.DbContext.Set<Comment>()
            .Where(r => r.Id == request.CommentId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.ViolationCount, r => r.ViolationCount + 1)
                .SetProperty(r => r.IsViolationChecked, false));

        return violation;
    }
}
