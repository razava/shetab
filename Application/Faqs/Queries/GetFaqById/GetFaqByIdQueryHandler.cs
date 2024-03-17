using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Application.Faqs.Queries.GetFaqById;

internal class GetFaqByIdQueryHandler(IFaqRepository faqRepository) : IRequestHandler<GetFaqByIdQuery, Result<Faq>>
{

    public async Task<Result<Faq>> Handle(GetFaqByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await faqRepository.GetSingleAsync(f => f.Id == request.Id, false);
        if (result == null)
            return NotFoundErrors.FAQ;
        return result;

    }
}
