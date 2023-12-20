using Application.Polls.Queries.GetPollsQuery;
using Domain.Models.Relational.PollAggregate;
using MediatR;

namespace Application.Polls.Queries.GetPollsByIdQuery;

public record GetPollsByIdQuery(int Id, string userId) : IRequest<GetPollsResponse>;
