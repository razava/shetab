namespace Domain.Models.Relational;

public class Complaint
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = null!;
    public string ComplainantId { get; set; } = null!;
    public ApplicationUser Complainant { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? Finished { get; set; }
    public DateTime CurrentDeadline { get; set; }
    public DateTime Deadline { get; set; }
    public List<ComplaintLog> Logs { get; set; } = new List<ComplaintLog>();
    public int CategoryId { get; set; }
    public ComplaintCategory Category { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public int? CurrentUnitId { get; set; }
    public ComplaintOrganizationalUnit CurrentUnit { get; set; } = null!;
    public int InstanceId { get; set; }
    public ShahrbinInstance Instance { get; set; } = null!;
    public ComplaintState Status { get; set; }
    public int? Rating { get; set; }
    public ICollection<Media> Medias { get; set; } = new List<Media>();

}


