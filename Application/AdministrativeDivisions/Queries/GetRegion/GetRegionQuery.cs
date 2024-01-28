using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetRegion;

public sealed record GetRegionQuery(int CityId) : IRequest<Result<List<Region>>>;

