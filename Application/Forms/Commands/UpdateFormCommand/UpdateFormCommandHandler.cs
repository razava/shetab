using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Commands.UpdateFormCommand;

internal sealed class UpdateFormCommandHandler(
    IFormRepository formRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFormCommand, Result<Form>>
{

    public async Task<Result<Form>> Handle(UpdateFormCommand request, CancellationToken cancellationToken)
    {
        var form = await formRepository.GetSingleAsync(q => q.Id == request.Id);
        if (form is null)
            return NotFoundErrors.Form;

        form.Update(request.Title, request.Elements);

        formRepository.Update(form);
        await unitOfWork.SaveAsync();

        return form;
    }
}
