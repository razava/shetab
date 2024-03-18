namespace Application.Processes.Common;

public record GetExecutiveActorsResponse(
    int Id,
    string FirstName,
    string LastName,
    string Title,
    string DisplayName);