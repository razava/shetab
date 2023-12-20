using Application.Comments.Commands.ReplyComment;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Commands.UpdateComment;

internal class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, bool>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    async Task<bool> IRequestHandler<UpdateCommentCommand, bool>.Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetSingleAsync(c => c.Id == request.CommentId);
        if (comment is null)
        {
            throw new NotFoundException("Comment");
        }
        comment.Text = request.Content;
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
