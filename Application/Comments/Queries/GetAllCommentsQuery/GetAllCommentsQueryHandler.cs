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
        (request.FilterModel.FromDate == null || c.DateTime >= request.FilterModel.FromDate)
        && (request.FilterModel.ToDate == null || c.DateTime <= request.FilterModel.ToDate)
        && (request.FilterModel.Categories == null || c.Report == null || request.FilterModel.Categories.Contains(c.Report.CategoryId))
        && (request.FilterModel.Query == null || c.Report == null || c.Report.TrackingNumber.Contains(request.FilterModel.Query)));

        
        var result = await commentRepository.GetAll(filter, request.PagingInfo, GetCommentsResponse.GetSelector());

        return result;
    }
}
