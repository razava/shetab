using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using SharedKernel.Successes;

namespace Application.Faqs.Commands.UpdateFaq;

internal sealed class UpdateFaqCommandHandler(
    IFaqRepository faqRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFaqCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = await faqRepository.GetSingleAsync(q => q.Id == request.Id);
        if (faq is null)
            return NotFoundErrors.FAQ;

        faq.Update(request.Question, request.Answer, request.IsDeleted);

        faqRepository.Update(faq);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.Faq);
    }
}
