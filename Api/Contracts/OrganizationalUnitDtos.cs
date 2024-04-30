using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;



public record OrganizationalUnitCreateDto(
    [MaxLength(256)]
    string Title,
    [Required] [MaxLength(256)]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds);


public record OrganizationalUnitUpdateDto(
    [MaxLength(256)]
    string Title,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds);

