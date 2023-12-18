using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Queries.GetAllCommentsQuery;

internal sealed class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, PagedList<Comment>>
{
    private readonly ICommentRepository _commentRepository;

    public GetAllCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<PagedList<Comment>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _commentRepository.GetPagedAsync(
            request.PagingInfo, 
            c => c.ShahrbinInstanceId == request.InstanceId && c.IsSeen == request.IsSeen,
            false, 
            o => o.OrderBy(c => c.DateTime),
            "User");

        return result;
    }
}
