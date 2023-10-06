using Domain.Models.Relational.IdentityAggregate;

namespace Domain.Models.Relational.ProcessAggregate;

public class ProcessStage
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public int Order { get; set; }
    //TODO: I think this prop is redundant and should be removed.
    public string Status { get; set; } = null!;
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    //This property is added to determine which role actors are in. This is used to find possible sources for each user.
    public string DisplayRoleId { get; set; } = null!;
    public ApplicationRole DisplayRole { get; set; } = null!;
}
