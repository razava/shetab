using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;

namespace Domain.Models.Relational;

public class OrganizationalUnit : BaseModel
{
    public int Id { get; set; }
    public OrganizationalUnitType? Type { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public int? ActorId { get; set; }
    public Actor Actor { get; set; } = null!;
    public string Title { get; set; } = null!;
    //public int? ParentId { get; set; }
    //public OrganizationalUnit Parent { get; set; }
    //TODO: Why is this relation n-m? what about 1-n?
    [InverseProperty("ParentOrganizationalUnits")]
    public List<OrganizationalUnit> OrganizationalUnits { get; set; } = new List<OrganizationalUnit>();
    [InverseProperty("OrganizationalUnits")]
    public List<OrganizationalUnit> ParentOrganizationalUnits { get; set; } = new List<OrganizationalUnit>();
}


