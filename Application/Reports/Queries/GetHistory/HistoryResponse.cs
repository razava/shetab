using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Reports.Queries.GetHistory;

public class HistoryResponse
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<Media> Attachments { get; set; } = new List<Media>();
    public ProcessReason Reason { get; set; } = default!;
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public ActorResponse Actor { get; set; } = default!;   //todo : Handle from Application Layer
}

public class ActorResponse
{
    //todo : is this correct that set ? for fix warnings here?
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
}
