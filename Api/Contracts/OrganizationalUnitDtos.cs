using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;



public record OrganizationalUnitCreateDto(
    [MaxLength(32)]
    string Title,
    [Required] [MaxLength(32)]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds);


public record OrganizationalUnitUpdateDto(
    [MaxLength(32)]
    string Title,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds);

