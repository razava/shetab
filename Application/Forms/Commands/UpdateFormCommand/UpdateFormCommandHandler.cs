using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
using Domain.Models.Relational.ReportAggregate;
using Mapster;

namespace Application.Forms.Commands.UpdateFormCommand;

internal sealed class UpdateFormCommandHandler(
    IFormRepository formRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFormCommand, Result<FormResponse>>
{

    public async Task<Result<FormResponse>> Handle(UpdateFormCommand request, CancellationToken cancellationToken)
    {
        var form = await formRepository.GetSingleAsync(q => q.Id == request.Id);
        if (form is null)
            return NotFoundErrors.Form;

        form.Update(request.Title, request.Elements?.Select(e => e.GetFormElement()).ToList());

        formRepository.Update(form);
        await unitOfWork.SaveAsync();

        return form.Adapt<FormResponse>();
    }
}
