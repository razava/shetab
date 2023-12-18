using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Queries.GetCommentsQuery;

public record GetCommentsQuery(PagingInfo PagingInfo, int InstanceId, bool IsSeen = false) : IRequest<List<Comment>>;
