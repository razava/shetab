using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Faqs.Queries.GetFaq;

internal class GetFaqQueryHandler(IFaqRepository faqRepository) : IRequestHandler<GetFaqQuery, Result<List<GetFaqsResponse>>>
{
    public async Task<Result<List<GetFaqsResponse>>> Handle(GetFaqQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Faq, bool>>? filter = f =>
            (request.ReturnAll || f.IsDeleted == false) && f.ShahrbinInstanceId == request.InstanceId;

        var result = await faqRepository.GetFaqs(filter, GetFaqsResponse.GetSelector());

        return result;
    }
}
