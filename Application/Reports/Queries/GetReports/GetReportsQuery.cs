using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReports;

public sealed record GetReportsQuery(
    PagingInfo PagingInfo,
    string UserId,
    string FromRoleId,
    int InstanceId) : IRequest<PagedList<Report>>;

