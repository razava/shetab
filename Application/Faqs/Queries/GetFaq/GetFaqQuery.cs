using Domain.Models.Relational;

namespace Application.Faqs.Queries.GetFaq;

public record GetFaqQuery(int InstanceId, bool ReturnAll = false) : IRequest<Result<List<Faq>>>;
