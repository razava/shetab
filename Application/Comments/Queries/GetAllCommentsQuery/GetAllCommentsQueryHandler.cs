using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Queries.GetAllCommentsQuery;

internal sealed class GetAllCommentsQueryHandler(ICommentRepository commentRepository) : IRequestHandler<GetAllCommentsQuery, Result<PagedList<Comment>>>
{
    public async Task<Result<PagedList<Comment>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        System.Linq.Expressions.Expression<Func<Comment, bool>>? filter = c =>
        c.ShahrbinInstanceId == request.InstanceId && c.IsSeen == request.IsSeen
        && ((request.FilterModel == null) ||
        (request.FilterModel.SentFromDate == null || c.DateTime >= request.FilterModel.SentFromDate)
        && (request.FilterModel.SentToDate == null || c.DateTime <= request.FilterModel.SentToDate)
        && (request.FilterModel.CategoryIds == null || c.Report == null || request.FilterModel.CategoryIds.Contains(c.Report.CategoryId))
        && (request.FilterModel.Query == null || c.Report == null || c.Report.TrackingNumber.Contains(request.FilterModel.Query)));

        var result = await commentRepository.GetPagedAsync(
            request.PagingInfo, 
            //c => c.ShahrbinInstanceId == request.InstanceId && c.IsSeen == request.IsSeen,
            filter,
            false, 
            o => o.OrderBy(c => c.DateTime),
            "User");

        return result;
    }
}
