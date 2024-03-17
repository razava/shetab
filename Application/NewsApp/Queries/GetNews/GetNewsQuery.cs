using Application.NewsApp.Common;

namespace Application.NewsApp.Queries.GetNews;

public record GetNewsQuery(int InstanceId, bool ReturnAll = false) 
    : IRequest<Result<List<GetNewsResponse>>>;
