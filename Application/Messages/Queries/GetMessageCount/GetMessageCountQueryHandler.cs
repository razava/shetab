using Application.Common.Interfaces.Persistence;

namespace Application.Messages.Queries.GetMessageCount;

internal sealed class GetMessageCountQueryHandler(
    IMessageRepository messageRepository)
    : IRequestHandler<GetMessageCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(
        GetMessageCountQuery request,
        CancellationToken cancellationToken)
    {
        var result = await messageRepository.GetMessageCount(request.UserId, request.From);
        return result;
    }
}
