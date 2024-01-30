using Application.Comments.Commands.ReplyComment;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

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
