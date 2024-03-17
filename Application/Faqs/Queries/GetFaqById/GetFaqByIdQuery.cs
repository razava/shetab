using Domain.Models.Relational;

namespace Application.Faqs.Queries.GetFaqById;

public record GetFaqByIdQuery(int Id) : IRequest<Result<Faq>>;


