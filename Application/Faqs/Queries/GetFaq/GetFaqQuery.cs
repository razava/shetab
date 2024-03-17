using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Faqs.Queries.GetFaq;

public record GetFaqQuery(int InstanceId, bool ReturnAll = false) : IRequest<Result<List<GetFaqsResponse>>>;


public record GetFaqsResponse(
    int Id,
    string Question,
    string Answer,
    bool IsDeleted)
{
    public static Expression<Func<Faq, GetFaqsResponse>> GetSelector()
    {
        Expression<Func<Faq, GetFaqsResponse>> selector
            = faq => new GetFaqsResponse(
                faq.Id,
                faq.Question,
                faq.Answer,
                faq.IsDeleted);
        return selector;
    }
}