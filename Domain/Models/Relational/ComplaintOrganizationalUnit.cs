using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;

namespace Domain.Models.Relational;

public class ComplaintOrganizationalUnit
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public ComplaintOrganizationalUnit Parent { get; set; }
    public List<ComplaintOrganizationalUnit> Children { get; set; } = new List<ComplaintOrganizationalUnit>();
    public string ComplaintInspectorId { get; set; }
    public ApplicationUser ComplaintInspector { get; set; }
    public int? InstanceId { get; set; }
    public ShahrbinInstance Instance { get; set; }
}