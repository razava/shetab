using Application.Common.Interfaces.Persistence;
using Application.Messages.Common;

namespace Application.Messages.Queries.GetMessages;

internal sealed class GetMessagesQueryHandler(
    IMessageRepository messageRepository) 
    : IRequestHandler<GetMessagesQuery, Result<PagedList<MessageResponse>>>
{
    public async Task<Result<PagedList<MessageResponse>>> Handle(
        GetMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await messageRepository.GetMessages(
            request.UserId,
            MessageResponse.GetSelector(),
            request.PagingInfo);

        return result;
    }
}
