using Application.Common.Interfaces.Persistence;
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
        _commentRepository.Delete(request.CommentId);
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
