namespace Application.Processes.Queries.GetProcessByIdQuery;

internal partial class GetProcessByIdQueryHandler
{
    public record GetProcessByIdResponse(int Id, string Code, string Title, List<int> ActorIds);
}
