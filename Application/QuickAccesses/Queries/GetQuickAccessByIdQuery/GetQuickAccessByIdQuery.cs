using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccessByIdQuery;

public record GetQuickAccessByIdQuery(int id) : IRequest<QuickAccess>;

