using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;

namespace Application.Forms.Queries.GetForm;

internal class GetFormQueryHandler(IFormRepository formRepository)
    : IRequestHandler<GetFormQuery, Result<List<FormListItemResponse>>>
{
    public async Task<Result<List<FormListItemResponse>>> Handle(
        GetFormQuery request,
        CancellationToken cancellationToken)
    {

        var result = await formRepository.GetForms(request.InstanceId, FormListItemResponse.GetSelector());
        return result;
    }
}
