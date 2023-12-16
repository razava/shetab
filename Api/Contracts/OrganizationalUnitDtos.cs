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


public record OrganizationalUnitCreateDto(
    string Title,
    string Username,
    string Password,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds);


public record OrganizationalUnitUpdateDto(
    string Title,
    List<int> ExecutiveActorsIds,
    List<int> OrganizationalUnitsIds);

