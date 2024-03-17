using Domain.Models.Relational.Common;

namespace Application.AdministrativeDivisions.Queries.GetDistrict;

public sealed record GetDistrictQuery(int CountyId) : IRequest<Result<List<District>>>;

