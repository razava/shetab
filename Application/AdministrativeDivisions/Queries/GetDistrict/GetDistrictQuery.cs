using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

public sealed record GetDistrictQuery(int CountyId) : IRequest<List<District>>;

