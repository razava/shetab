using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Forms.Commands.UpdateForm;

internal sealed class UpdateFormCommandHandler(
    IFormRepository formRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFormCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateFormCommand request, CancellationToken cancellationToken)
    {
        var form = await formRepository.GetSingleAsync(q => q.Id == request.Id);
        if (form is null)
            return NotFoundErrors.Form;

        form.Update(request.Title, request.Elements?.Select(e => e.GetFormElement()).ToList());

        formRepository.Update(form);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.Form);
    }
}
