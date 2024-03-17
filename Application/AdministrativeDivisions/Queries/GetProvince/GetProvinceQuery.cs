using Domain.Models.Relational.Common;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

public sealed record GetProvinceQuery() : IRequest<Result<List<Province>>>;

