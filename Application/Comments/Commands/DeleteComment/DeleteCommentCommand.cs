using MediatR;

namespace Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(
    Guid CommentId,
    string UserId,
    List<string> UserRoles) : IRequest<bool>;