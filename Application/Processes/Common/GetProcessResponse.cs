using Domain.Models.Relational.ProcessAggregate;
using System.Linq.Expressions;

namespace Application.Processes.Common;

public record GetProcessResponse(
    int Id,
    string Title,
    List<int> ActorIds)
{
    public static Expression<Func<Process, GetProcessResponse>> GetSelector()
    {
        Expression<Func<Process, GetProcessResponse>> selector
            = process => new GetProcessResponse(
                process.Id,
                process.Title,
                process.Stages.Where(s => s.Name == "Executive").First().Actors.Select(a => a.Id).ToList());
        return selector;
    }
}
