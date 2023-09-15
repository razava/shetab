namespace Domain.Models.Relational;

public class ViolationType
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Title { get; set; } = null!;
    public int Threshold { get; set; }
}


