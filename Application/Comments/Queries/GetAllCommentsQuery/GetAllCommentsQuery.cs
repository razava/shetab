using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Comments.Queries.GetAllCommentsQuery;

public record GetAllCommentsQuery(PagingInfo PagingInfo,
    int InstanceId,
    CommentFilters? FilterModel = default!,
    bool IsSeen = false) : IRequest<Result<PagedList<GetCommentsResponse>>>;

public record GetCommentsResponse(
    Guid Id,
    GetShortUserResponse User,
    string Text,
    Guid? ReportId,
    DateTime DateTime)
{
    public static Expression<Func<Comment, GetCommentsResponse>> GetSelector()
    {
        Expression<Func<Comment, GetCommentsResponse>> selector
            = comment => new GetCommentsResponse(
                comment.Id,
                new GetShortUserResponse(comment.User.FirstName, comment.User.LastName, comment.User.UserName!, comment.User.Avatar),
                comment.Text,
                comment.ReportId,
                comment.DateTime);

        return selector;
    }
}

public record GetShortUserResponse(
    string FirstName,
    string LastName,
    string UserName,
    Media? Avatar);

public record CommentFilters(
    string? Query,
    List<int>? Categories,
    DateTime? FromDate,
    DateTime? ToDate);