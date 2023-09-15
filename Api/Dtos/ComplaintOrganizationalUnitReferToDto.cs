namespace Api.Dtos;
public class ComplaintOrganizationalUnitReferToDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDeadlineMandatory { get; set; }
}