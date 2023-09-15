namespace Domain.Models.Relational;

public class Chart : BaseModel
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int Code { get; set; }
    public string Title { get; set; } = null!;
    public List<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
}
