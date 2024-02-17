namespace Application.Messages.Queries.GetMessages;

public sealed record GetMessageCountQuery(
    DateTime From,
    string UserId) : IRequest<Result<int>>;
