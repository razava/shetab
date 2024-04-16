using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Successes;

namespace Application.Comments.Commands.DeleteComment;

internal class DeleteCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommentCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetSingleAsync(c => c.Id == request.CommentId);
        if (comment is null)
            return NotFoundErrors.Comment;

        if(!(comment.UserId == request.UserId || request.UserRoles.Contains(RoleNames.Operator)))
        {
            return AccessDeniedErrors.General;
        }
        commentRepository.Delete(comment);
        try
        {
            await unitOfWork.SaveAsync();
            await unitOfWork.DbContext.Set<Report>()
                .Where(r => r.Id == comment.ReportId)
                .ExecuteUpdateAsync(r => r.SetProperty(e => e.CommentsCount, e => e.CommentsCount - 1));
        }
        catch
        {
            return OperationErrors.General;
        }

        return ResultMethods.GetResult(true, DeleteSuccess.Comment);
    }
}
