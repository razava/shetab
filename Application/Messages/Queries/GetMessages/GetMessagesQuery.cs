using Application.Common.Interfaces.Persistence;
using Application.Messages.Common;

namespace Application.Messages.Queries.GetMessages;

public sealed record GetMessagesQuery(
    PagingInfo PagingInfo,
    string UserId) : IRequest<Result<PagedList<MessageResponse>>>;
