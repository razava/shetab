using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Queries.GetFaqByIdQuery;

public record GetFaqByIdQuery(int Id) : IRequest<Faq>;


