using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Queries.GetCommentsQuery;

internal sealed class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, List<Comment>>
{
    private readonly ICommentRepository _commentRepository;

    public GetCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<List<Comment>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _commentRepository.GetPagedAsync(
            request.PagingInfo, 
            c => c.ShahrbinInstanceId == request.InstanceId && request.IsSeen,
            false, 
            o => o.OrderBy(c => c.DateTime));

        return result.ToList();
    }
}
