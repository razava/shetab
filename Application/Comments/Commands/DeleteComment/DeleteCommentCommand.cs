using MediatR;

namespace Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(
    Guid CommentId) : IRequest<bool>;