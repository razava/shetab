using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using System.Linq.Expressions;

namespace Application.Faqs.Queries.GetFaqQuery;

internal class GetFaqQueryHandler(IFaqRepository faqRepository) : IRequestHandler<GetFaqQuery, Result<List<Faq>>>
{
    public async Task<Result<List<Faq>>> Handle(GetFaqQuery request, CancellationToken cancellationToken)
    {
        var result = await faqRepository.GetAsync(f => (request.ReturnAll || f.IsDeleted == false), false);

        return result.ToList();
    }
}
