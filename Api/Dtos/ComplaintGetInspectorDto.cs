using Domain.Models.Relational;

namespace Api.Dtos;

public class ComplaintGetInspectorDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; }
    public ApplicationUserDto Complainant { get; set; }
    public DateTime Created { get; private set; }
    public DateTime? Finished { get; set; }
    public DateTime? CurrentDeadline { get; set; }
    public DateTime Deadline { get; set; }
    public List<ComplaintLogGetDto> Logs { get; set; }
    public int CategoryId { get; set; }
    public ComplaintCategoryGetDto Category { get; set; }
    public string Description { get; set; } = string.Empty;
    public ComplaintOrganizationalUnitGetDto CurrentUnit { get; set; }
    public ShahrbinInstance Instance { get; set; }
    public List<ComplaintOrganizationalUnitReferToDto> ReferToOptions { get; set; }
    public ComplaintState Status { get; set; }
    public int? Rating { get; set; }

}
