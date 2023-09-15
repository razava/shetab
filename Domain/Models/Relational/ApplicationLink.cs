namespace Domain.Models.Relational;

public class ApplicationLink : BaseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Media? Image { get; set; }
    public DateTime Created { get; set; }
    public bool IsDeleted { get; set; }
}
