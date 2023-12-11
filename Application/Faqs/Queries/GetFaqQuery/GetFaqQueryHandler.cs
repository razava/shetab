using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using System.Linq.Expressions;

namespace Application.Faqs.Queries.GetFaqQuery;

internal class GetFaqQueryHandler : IRequestHandler<GetFaqQuery, List<Faq>>
{
    private readonly IFaqRepository _faqRepository;

    public GetFaqQueryHandler(IFaqRepository faqRepository)
    {
        _faqRepository = faqRepository;
    }

    public async Task<List<Faq>> Handle(GetFaqQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Faq, bool>>? filter = q => q.IsDeleted != true;


        var result = await _faqRepository.GetAsync(filter, false);

        return result.ToList();
    }
}
