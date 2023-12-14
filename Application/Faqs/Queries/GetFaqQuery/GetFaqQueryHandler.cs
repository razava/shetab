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
        var result = await _faqRepository.GetAsync(f => (request.ReturnAll || f.IsDeleted == false), false);

        return result.ToList();
    }
}
