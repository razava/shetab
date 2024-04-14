using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Setup.Commands.SpecifyReplyComments;

internal class SpecifyReplyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SpecifyReplyCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SpecifyReplyCommand request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var replyCommentIds = await context.Set<Comment>()
            .AsNoTracking()
            .Where(c => c.ShahrbinInstanceId == request.instanceId && c.ReplyId != null)
            .Select(c => c.ReplyId)
            .ToListAsync();

        var replyComments = await context.Set<Comment>()
            .Where(c => replyCommentIds.Contains(c.Id))
            .ToListAsync();

        foreach (var comment in replyComments)
        {
            comment.IsReply = true;
        }

        context.Set<Comment>().AttachRange(replyComments);
        await unitOfWork.SaveAsync();

        return true;
    }
}
