using Application.Common.Interfaces.Persistence;

namespace Application.Comments.Commands.UpdateComment;

internal class UpdateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetSingleAsync(c => c.Id == request.CommentId);
        if (comment is null)
        {
            return NotFoundErrors.Comment;
        }
        comment.Text = request.Content;
        commentRepository.Update(comment);
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
