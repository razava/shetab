using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Queries.GetFormQuery;

internal class GetFormQueryHandler(IFormRepository formRepository) 
    : IRequestHandler<GetFormQuery, Result<List<Form>>>
{
    public async Task<Result<List<Form>>> Handle(GetFormQuery request, CancellationToken cancellationToken)
    {
        var result = await formRepository.GetAsync(null, false);

        return result.ToList();
    }
}
