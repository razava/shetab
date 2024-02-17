using Application.Common.Interfaces.Persistence;
using Application.Messages.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Messages.Queries.GetMessages;

internal sealed class GetMessagesQueryHandler(
    IUnitOfWork unitOfWork) 
    : IRequestHandler<GetMessagesQuery, Result<PagedList<MessageResponse>>>
{
    public async Task<Result<PagedList<MessageResponse>>> Handle(
        GetMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Message>()
            .Where(m => m.Recepient.Type == RecepientType.Person
                && m.Recepient.ToId == request.UserId)
            .OrderByDescending(m => m.DateTime)
            .Select(MessageResponse.GetSelector());

        var result = await PagedList<MessageResponse>.ToPagedList(
            query,
            request.PagingInfo.PageNumber,
            request.PagingInfo.PageSize);

        return result;
    }
}
