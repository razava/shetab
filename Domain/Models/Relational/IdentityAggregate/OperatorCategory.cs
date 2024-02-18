namespace Domain.Models.Relational.IdentityAggregate;

public class OperatorCategory
{
    public string OperatorId { get; set; } = null!;
    public ApplicationUser Operator { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}