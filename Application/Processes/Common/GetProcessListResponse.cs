using Domain.Models.Relational.ProcessAggregate;
using System.Linq.Expressions;

namespace Application.Processes.Common;

public record GetProcessListResponse(
    int Id,
    string Title,
    string Code)
{
    public static Expression<Func<Process, GetProcessListResponse>> GetSelector()
    {
        Expression<Func<Process, GetProcessListResponse>> selector
            = process => new GetProcessListResponse(
                process.Id,
                process.Title,
                process.Code);
        return selector;
    }
}
