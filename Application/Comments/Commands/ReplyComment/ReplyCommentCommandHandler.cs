using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Commands.ReplyComment;

internal class ReplyCommentCommandHandler : IRequestHandler<ReplyCommentCommand, bool>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReplyCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    async Task<bool> IRequestHandler<ReplyCommentCommand, bool>.Handle(ReplyCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetSingleAsync(c => c.Id == request.CommentId, true, "Reply");
        if (comment is null)
        {
            throw new Exception("Not found.");
        }
        if (comment.Reply is not null)
        {
            throw new Exception("This comment has a reply.");
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
            };
        }
        
        comment.IsSeen = true;
        _commentRepository.Update(comment);
        try
        {
            await _unitOfWork.SaveAsync();
        }
        catch
        {
            return false;
        }

        return true;
    }
}
