using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.GetNews;

public record GetNewsQuery(int InstanceId) : IRequest<List<News>>;
