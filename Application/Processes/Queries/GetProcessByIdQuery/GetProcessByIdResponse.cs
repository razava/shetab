namespace Application.Processes.Queries.GetProcessByIdQuery;

public record GetProcessByIdResponse(int Id, string Code, string Title, List<int> ActorIds);