using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetNearestReports;

public sealed record GetNearestReportsQuery(
    PagingInfo PagingInfo,
    int instanceId) : IRequest<PagedList<Report>>;

