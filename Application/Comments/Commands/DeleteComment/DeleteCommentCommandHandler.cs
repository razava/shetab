using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Commands.DeleteComment;

internal class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    async Task<bool> IRequestHandler<DeleteCommentCommand, bool>.Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetSingleAsync(c => c.Id == request.CommentId);
        if (comment is null)
            throw new NotFoundException("نظر");

        if(!(comment.UserId == request.UserId || request.UserRoles.Contains(RoleNames.Operator)))
        {
            return false;
        }
        _commentRepository.Delete(comment);
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
