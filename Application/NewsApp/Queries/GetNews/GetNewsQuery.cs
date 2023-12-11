using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.UpdateNewsCommand;

public record GetNewsQuery(int InstanceId) : IRequest<List<News>>;
