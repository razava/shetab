using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Successes;

namespace Application.Violations.Commands.CheckCommentViolation;

internal class CheckCommentViolationCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CheckCommentViolationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckCommentViolationCommand request, CancellationToken cancellationToken)
    {
        var comment = await unitOfWork.DbContext.Set<Comment>()
                .Where(r => r.Id == request.CommentId)
                .FirstOrDefaultAsync();

        if (comment == null)
            return NotFoundErrors.Comment;

        if (request.Action == ViolationCheckResult.NoAction)
        {

        }
        else if (request.Action == ViolationCheckResult.Deleted)
        {
            comment.IsDeleted = true;
        }
        else if (request.Action == ViolationCheckResult.Corrected)
        {
            comment.Text = request.Text ?? string.Empty;
        }

        comment.IsViolationChecked = true;
        await unitOfWork.SaveAsync();


        var now = DateTime.UtcNow;
        await unitOfWork.DbContext.Set<Violation>()
                .Where(v => v.CommentId == request.CommentId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.ViolationCheckResult, request.Action)
                    .SetProperty(a => a.ViolatoinCheckDateTime, now));

        return ResultMethods.GetResult(true, OperationSuccess.ViolationReview);
    }
}
