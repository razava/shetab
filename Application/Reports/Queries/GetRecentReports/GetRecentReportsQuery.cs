using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetRecentReports;

public sealed record GetRecentReportsQuery(
    PagingInfo PagingInfo,
    int instanceId,
    string UserId) : IRequest<Result<PagedList<Report>>>;

