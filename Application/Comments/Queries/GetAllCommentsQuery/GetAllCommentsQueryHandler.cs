using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Comments.Queries.GetAllCommentsQuery;

internal sealed class GetAllCommentsQueryHandler(ICommentRepository commentRepository) : IRequestHandler<GetAllCommentsQuery, Result<PagedList<GetCommentsResponse>>>
{
    public async Task<Result<PagedList<GetCommentsResponse>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        System.Linq.Expressions.Expression<Func<Comment, bool>>? filter = c =>
        c.ShahrbinInstanceId == request.InstanceId && c.IsSeen == request.IsSeen && !c.IsReply
        && ((request.FilterModel == null) ||
        (request.FilterModel.SentFromDate == null || c.DateTime >= request.FilterModel.SentFromDate)
        && (request.FilterModel.SentToDate == null || c.DateTime <= request.FilterModel.SentToDate)
        && (request.FilterModel.CategoryIds == null || c.Report == null || request.FilterModel.CategoryIds.Contains(c.Report.CategoryId))
        && (request.FilterModel.Query == null || c.Report == null || c.Report.TrackingNumber.Contains(request.FilterModel.Query)));

        
        var result = await commentRepository.GetAll(filter, request.PagingInfo, GetCommentsResponse.GetSelector());

        return result;
    }
}
