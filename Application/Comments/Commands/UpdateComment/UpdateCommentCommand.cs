﻿namespace Application.Comments.Commands.UpdateComment;

public record UpdateCommentCommand(
    string UserId,
    Guid CommentId,
    string Content) : IRequest<Result<bool>>;