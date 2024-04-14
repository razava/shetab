using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
using Domain.Models.Relational.ReportAggregate;
using SharedKernel.Successes;

namespace Application.Forms.Commands.AddForm;

internal sealed class AddFormCommandHandler(
    IFormRepository formRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddFormCommand, Result<FormResponse>>
{

    public async Task<Result<FormResponse>> Handle(AddFormCommand request, CancellationToken cancellationToken)
    {
        var form = Form.Create(
            request.InstanceId,
            request.Title,
            request.Elements.Select(e => e.GetFormElement()).ToList());

        formRepository.Insert(form);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(FormResponse.FromForm(form)!, CreationSuccess.Form);
    }
}
