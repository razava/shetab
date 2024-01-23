using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;



public record GetOrganizationalUnitListDto(
    int Id,
    [MaxLength(32)]
    string Title);


public record GetOrganizationalUnitDto(
    int Id,
    [MaxLength(32)]
    string Title,
    int? ActorId,
    OrganizationalUnitType? Type,
    List<GetOrganizationalUnitDto> OrganizationalUnits);


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

