namespace Application.Comments.Commands.ReplyComment;

public record ReplyCommentCommand(
    string UserId,
    Guid CommentId,
    string Content) : IRequest<Result<bool>>;