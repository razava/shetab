using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Reports.Queries.GetComments;

public sealed record GetCommentsQuery(
    Guid ReportId,
    PagingInfo PagingInfo) : IRequest<PagedList<Comment>>;

