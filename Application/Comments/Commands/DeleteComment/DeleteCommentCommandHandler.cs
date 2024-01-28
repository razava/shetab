using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

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
        }
        catch
        {
            return OperationErrors.General;
        }

        return true;
    }
}
