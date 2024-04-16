using Application.Faqs.Queries.GetFaq;

namespace Application.Faqs.Queries.GetFaqById;

public record GetFaqByIdQuery(int Id) : IRequest<Result<GetFaqsResponse>>;


