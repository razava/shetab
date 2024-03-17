using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Common;

namespace Application.NewsApp.Queries.GetNews;

public record GetNewsQuery(PagingInfo pagingInfo, int InstanceId, bool ReturnAll = false) 
    : IRequest<Result<PagedList<GetNewsResponse>>>;
