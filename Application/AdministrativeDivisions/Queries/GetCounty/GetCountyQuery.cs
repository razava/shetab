using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

public sealed record GetCountyQuery(int ProviceId) : IRequest<Result<List<County>>>;

