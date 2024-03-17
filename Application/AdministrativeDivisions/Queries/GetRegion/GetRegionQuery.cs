using Domain.Models.Relational.Common;

namespace Application.AdministrativeDivisions.Queries.GetRegion;

public sealed record GetRegionQuery(int CityId) : IRequest<Result<List<GetRegionResponse>>>;

public record GetRegionResponse(
    int Id,
    string Name,
    string ParsimapCode);