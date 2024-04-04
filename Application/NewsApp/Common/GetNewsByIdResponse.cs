using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.NewsApp.Common;

public record GetNewsByIdResponse(
    int Id,
    string Title,
    string Description,
    string Url,
    Media? ImageFile,
    DateTime Created,
    bool IsDeleted)
{
    public static Expression<Func<News, GetNewsByIdResponse>> GetSelector()
    {
        Expression<Func<News, GetNewsByIdResponse>> selector
            = news => new GetNewsByIdResponse(
                news.Id,
                news.Title,
                news.Description,
                news.Url,
                news.Image,
                news.Created,
                news.IsDeleted);
        return selector;
    }
}