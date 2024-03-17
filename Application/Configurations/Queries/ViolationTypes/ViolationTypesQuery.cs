using Domain.Models.Relational;

namespace Application.Configurations.Queries.ViolationTypes;

public record ViolationTypesQuery():IRequest<Result<List<ViolationType>>>;

