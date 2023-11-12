using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Commands.CreateComment;

internal class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, bool>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    async Task<bool> IRequestHandler<CreateCommentCommand, bool>.Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment()
        {
            ReportId = request.ReportId,
            Text = request.Content,
            ShahrbinInstanceId = request.InstanceId,
            UserId = request.UserId,
            DateTime = DateTime.UtcNow
        };
        _commentRepository.Insert(comment);
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
