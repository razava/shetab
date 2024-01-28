using MediatR;

namespace Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(
    int InstanceId,
    string UserId,
    Guid ReportId,
    string Content) : IRequest<Result<bool>>;