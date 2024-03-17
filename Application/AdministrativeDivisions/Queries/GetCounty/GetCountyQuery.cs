using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetCounty;

public sealed record GetCountyQuery(int ProviceId) : IRequest<Result<List<County>>>;

