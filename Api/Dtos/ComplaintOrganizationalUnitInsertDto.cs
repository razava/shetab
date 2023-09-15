namespace Api.Dtos;

public class ComplaintOrganizationalUnitInsertDto
{
    public string Title { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public ApplicationUserDto ComplaintInspector { get; set; }
    public string InspectorUsername { get; set; }
    public string InspectorPassword { get; set; }
    public string InspectorFirstName { get; set; }
    public string InspectorLastName { get; set; }
    public int InstanceId { get; set; }
}