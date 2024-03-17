using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Faqs.Queries.GetFaq;

internal class GetFaqQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetFaqQuery, Result<List<GetFaqsResponse>>>
{
    public async Task<Result<List<GetFaqsResponse>>> Handle(GetFaqQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<Faq>()
            .Where(f => request.ReturnAll || f.IsDeleted == false)
            .Select(GetFaqsResponse.GetSelector())
            .ToListAsync();

        return result;
    }
}
