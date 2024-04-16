using Application.Common.Interfaces.Persistence;
using Application.Faqs.Queries.GetFaq;
using Mapster;

namespace Application.Faqs.Queries.GetFaqById;

internal class GetFaqByIdQueryHandler(IFaqRepository faqRepository) : IRequestHandler<GetFaqByIdQuery, Result<GetFaqsResponse>>
{

    public async Task<Result<GetFaqsResponse>> Handle(GetFaqByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await faqRepository.GetSingleAsync(f => f.Id == request.Id, false);
        if (result == null)
            return NotFoundErrors.FAQ;
        return result.Adapt<GetFaqsResponse>();

    }
}
