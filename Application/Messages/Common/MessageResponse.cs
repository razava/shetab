using Application.Reports.Common;
using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Messages.Common;

public record MessageResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime DateTime,
    Guid SubjectId)
{
    public static MessageResponse FromMessage(Message message)
    {
        return new MessageResponse(
                message.Id,
                message.Title,
                message.Content,
                message.DateTime,
                message.SubjectId);
    }
    public static Expression<Func<Message, MessageResponse>> GetSelector()
    {
        Expression<Func<Message, MessageResponse>> selector
            = message => new MessageResponse(
                message.Id,
                message.Title,
                message.Content,
                message.DateTime,
                message.SubjectId);
        return selector;
    }
}
