namespace Domain.Models.Relational;

public class ComplaintLog
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime CurrentDeadline { get; set; }
    public string ActorId { get; set; }
    public ApplicationUser Actor { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}
