using Domain.Models.Relational;

namespace Application.QuickAccesses.Queries.GetQuickAccessById;

public record GetQuickAccessByIdQuery(int id) : IRequest<Result<QuickAccess>>;

