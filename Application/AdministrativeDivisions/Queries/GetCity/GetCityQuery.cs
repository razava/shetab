using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

public sealed record GetCityQuery(int DistrictId) : IRequest<Result<List<City>>>;

