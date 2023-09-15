namespace Domain.Models.Relational;

public class RevisionUnit
{
    public int Id { get; set; }
    public string RevisorId { get; set; } = null!;
    public ApplicationUser Revisor { get; set; } = null!;

}
