using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReports;

public sealed record GetAllReportsQuery(
    PagingInfo PagingInfo,
    int instanceId) : IRequest<PagedList<Report>>;

