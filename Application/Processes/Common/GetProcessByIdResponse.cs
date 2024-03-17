namespace Application.Processes.Common;

public record GetProcessByIdResponse(int Id, string Code, string Title, List<int> ActorIds);