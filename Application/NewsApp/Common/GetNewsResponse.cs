using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.NewsApp.Common;

public record GetNewsResponse(
    int Id,
    string Title,
    string Description,
    string Url,
    Media? ImageFile,
    DateTime Created,
    bool IsDeleted)
{
    public static Expression<Func<News, GetNewsResponse>> GetSelector()
    {
        Expression<Func<News, GetNewsResponse>> selector
            = news => new GetNewsResponse(
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