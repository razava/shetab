using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Queries.GetFaqByIdQuery;

internal class GetFaqByIdQueryHandler : IRequestHandler<GetFaqByIdQuery, Faq>
{
    private readonly IFaqRepository _faqRepository;

    public GetFaqByIdQueryHandler(IFaqRepository faqRepository)
    {
        _faqRepository = faqRepository;
    }
    public async Task<Faq> Handle(GetFaqByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _faqRepository.GetSingleAsync(f => f.Id == request.Id, false);
        if (result == null)
            throw new NotFoundException("FAQ");
        return result;

    }
}
