using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReportById;

public sealed record GetReportByIdQuery(
    Guid id,
    string userId,
    int instanceId) : IRequest<Result<Report>>;

