using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

public sealed record GetRegionQuery(int CityId) : IRequest<List<Region>>;

