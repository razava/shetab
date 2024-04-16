using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using SharedKernel.Successes;

namespace Application.Comments.Commands.ReplyComment;

internal class ReplyCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork) : IRequestHandler<ReplyCommentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ReplyCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetSingleAsync(c => c.Id == request.CommentId, true, "Reply");
        if (comment is null)
        {
            return NotFoundErrors.Comment;
        }
        if (comment.Reply is not null)
        {
            return OperationErrors.CommentHasReply;
        }
        if (!string.IsNullOrEmpty(request.Content))
        {
            comment.Reply = new Comment()
            {
                ReportId = comment.ReportId,
                UserId = request.UserId,
                DateTime = DateTime.UtcNow,
                ShahrbinInstanceId = comment.ShahrbinInstanceId,
                Text = request.Content,
                IsReply = true
            };
        }
        
        comment.IsSeen = true;
        commentRepository.Update(comment);
        try
        {
            await unitOfWork.SaveAsync();
        }
        catch
        {
            return OperationErrors.General;
        }

        return ResultMethods.GetResult(true, OperationSuccess.ReplyComment);
    }
}
