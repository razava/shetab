using Domain.Models.Relational.Common;

namespace Application.AdministrativeDivisions.Queries.GetCity;

public sealed record GetCityQuery(int DistrictId) : IRequest<Result<List<City>>>;

