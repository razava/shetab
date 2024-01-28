using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Commands.CreateComment;

internal class CreateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment()
        {
            ReportId = request.ReportId,
            Text = request.Content,
            ShahrbinInstanceId = request.InstanceId,
            UserId = request.UserId,
            DateTime = DateTime.UtcNow
        };
        commentRepository.Insert(comment);
        try
        {
            await unitOfWork.SaveAsync();
        }
        catch
        {
            return OperationErrors.General;
        }
        
        return true;
    }
}
