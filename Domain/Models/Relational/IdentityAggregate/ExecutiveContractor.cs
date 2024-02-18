namespace Domain.Models.Relational.IdentityAggregate;

public class ExecutiveContractor
{
    public string ExecutiveId { get; set; } = null!;
    public ApplicationUser Executive { get; set; } = null!;
    public string ContractorId { get; set; } = null!;
    public ApplicationUser Contractor { get; set; } = null!;
}
