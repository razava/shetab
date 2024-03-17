using Application.QuickAccesses.Common;

namespace Application.QuickAccesses.Queries.GetQuickAccessById;

public record GetQuickAccessByIdQuery(int id) : IRequest<Result<AdminGetQuickAccessResponse>>;

