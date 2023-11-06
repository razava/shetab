using Domain.Models.Relational;
using MediatR;

namespace Application.Configurations.Queries.ViolationTypes;

public record ViolationTypesQuery():IRequest<List<ViolationType>>;

