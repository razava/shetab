using Domain.Models.Relational;
using MediatR;

namespace Application.NewsApp.Queries.GetNewsById;

public record GetNewsByIdQuery(int Id) : IRequest<News>;
