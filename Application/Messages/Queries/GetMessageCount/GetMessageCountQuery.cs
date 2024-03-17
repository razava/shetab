namespace Application.Messages.Queries.GetMessageCount;

public sealed record GetMessageCountQuery(
    DateTime From,
    string UserId) : IRequest<Result<int>>;
