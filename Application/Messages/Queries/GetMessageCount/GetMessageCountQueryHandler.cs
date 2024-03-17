using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Messages.Queries.GetMessageCount;

internal sealed class GetMessageCountQueryHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetMessageCountQuery, Result<int>>
{
    public async Task<Result<int>> Handle(
        GetMessageCountQuery request,
        CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Message>()
            .Where(m => m.Recepient.Type == RecepientType.Person
                && m.Recepient.ToId == request.UserId
                && m.DateTime > request.From);

        var result = await query.CountAsync();

        return result;
    }
}
