namespace Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(
    string UserId,
    Guid ReportId,
    string Content) : IRequest<Result<bool>>;