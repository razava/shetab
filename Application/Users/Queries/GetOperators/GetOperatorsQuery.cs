namespace Application.Users.Queries.GetOperators;

public record GetOperatorsQuery(
    int InstanceId) : IRequest<Result<List<GetSelectListResponse<string>>>>;

public record GetSelectListResponse<T>(string Text, T Value);