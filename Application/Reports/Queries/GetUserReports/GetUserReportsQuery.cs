using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetUserReports;

public sealed record GetUserReportsQuery(
    PagingInfo PagingInfo,
    string UserId) : IRequest<Result<PagedList<Report>>>;

