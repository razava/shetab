using Application.Common.Interfaces.Persistence;
using DocumentFormat.OpenXml.InkML;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        await unitOfWork.DbContext.Set<Report>()
                .Where(r => r.Id == request.ReportId)
                .ExecuteUpdateAsync(r => r.SetProperty(e => e.CommentsCount, e => e.CommentsCount + 1));
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
