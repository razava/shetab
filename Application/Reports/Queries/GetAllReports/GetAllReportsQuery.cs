using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetAllReports;

public sealed record GetAllReportsQuery(
    PagingInfo PagingInfo,
    int instanceId,
    string UserId,
    List<string> UserRoles,
    FilterGetAllReportsModel? FilterModel = default!) : IRequest<Result<PagedList<Report>>>;

