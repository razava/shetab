namespace Api.Dtos;

public class ComplaintOrganizationalUnitUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int? InstanceId { get; set; }
}