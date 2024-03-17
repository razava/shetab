using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Reports.Queries.GetComments;

public sealed record GetCommentsQuery(
    Guid ReportId,
    string UserId,
    PagingInfo PagingInfo) : IRequest<Result<PagedList<GetReportCommentsResponse>>>;

public record GetReportCommentsResponse(
    Guid Id,
    string Text,
    DateTime DateTime,
    ApplicationUserRestrictedResponse User,
    Guid? ReportId,
    GetReportCommentsResponse? Reply,
    bool CanDelete)
{
    public static Expression<Func<Comment, GetReportCommentsResponse>> GetSelector(string userId)
    {
        Expression<Func<Comment, GetReportCommentsResponse>> selector
            = comment => new GetReportCommentsResponse(
                comment.Id,
                comment.Text,
                comment.DateTime,
                new ApplicationUserRestrictedResponse(
                    comment.User.Id,
                    comment.User.FirstName,
                    comment.User.LastName,
                    comment.User.Title,
                    comment.User.Organization,
                    comment.User.Avatar),
                comment.ReportId,
                comment.Reply == null ? null :
                    new GetReportCommentsResponse(comment.Reply.Id, comment.Reply.Text, comment.Reply.DateTime,
                    new ApplicationUserRestrictedResponse(
                        comment.Reply.User.Id,
                        comment.Reply.User.FirstName,
                        comment.Reply.User.LastName,
                        comment.Reply.User.Title,
                        comment.Reply.User.Organization,
                        comment.Reply.User.Avatar), comment.Reply.ReportId, null, comment.Reply.UserId == userId)
                    , comment.UserId == userId);
        return selector;
    }
}

public record ApplicationUserRestrictedResponse(
    string Id,
    string FirstName,
    string LastName,
    string Title,
    string Organization,
    Media? Avatar);
