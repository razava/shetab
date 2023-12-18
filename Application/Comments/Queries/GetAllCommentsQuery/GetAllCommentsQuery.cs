using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Queries.GetAllCommentsQuery;

public record GetAllCommentsQuery(PagingInfo PagingInfo, int InstanceId, bool IsSeen = false) : IRequest<PagedList<Comment>>;
