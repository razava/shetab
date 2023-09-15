namespace Domain.Models.Relational;

public class ComplaintCategory
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int? ParentId { get; set; }
    public ComplaintCategory Parent { get; set; } = null!;
    public List<ComplaintCategory> Children { get; set; } = new List<ComplaintCategory>();
}
