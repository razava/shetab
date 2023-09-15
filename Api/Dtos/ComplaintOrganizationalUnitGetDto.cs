namespace Api.Dtos;
public class ComplaintOrganizationalUnitGetDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public List<ComplaintOrganizationalUnitGetDto> Children { get; set; } = new List<ComplaintOrganizationalUnitGetDto>();
    public string ComplaintInspectorId { get; set; }
    public ApplicationUserDto ComplaintInspector { get; set; }
    public int? InstanceId { get; set; }
}