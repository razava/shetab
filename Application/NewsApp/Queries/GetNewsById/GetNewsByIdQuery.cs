using Application.NewsApp.Common;

namespace Application.NewsApp.Queries.GetNewsById;

public record GetNewsByIdQuery(int Id) : IRequest<Result<GetNewsResponse>>;
