using Domain.Models.Relational.Common;

namespace Api.Contracts;



public record GetOrganizationalUnitListDto(
    int Id,
    string Title);


public record GetOrganizationalUnitDto(
    int Id,
    string Title,
    int? ActorId,
    OrganizationalUnitType? Type,
    List<GetOrganizationalUnitDto> OrganizationalUnits);


public class OrganizationalUnitCreateDto
{
    public int Id { get; set; }
    public OrganizationalUnitType? Type { get; set; }
    public string UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int? ActorId { get; set; }
    public ActorDto Actor { get; set; }
    public string Title { get; set; } = default!;
    public List<int> OrganizationalUnitIds { get; set; } = new List<int>();
    public List<int> ActorIds { get; set; } = new List<int>();
}

public class OrganizationalUnitUpdateDto
{
    //public int Id { get; set; }
    public string Title { get; set; } = default!;
    public List<int> OrganizationalUnitIds { get; set; } = new List<int>();
    public List<int> ActorIds { get; set; } = new List<int>();
}

