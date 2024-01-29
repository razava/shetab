﻿using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Commands.UpdateFaqCommand;

internal sealed class UpdateFaqCommandHandler(
    IFaqRepository faqRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateFaqCommand, Result<Faq>>
{
    
    public async Task<Result<Faq>> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = await faqRepository.GetSingleAsync(q => q.Id == request.Id);
        if (faq is null)
            return NotFoundErrors.FAQ;

        faq.Update(request.Question, request.Answer, request.IsDeleted);

        faqRepository.Update(faq);
        await unitOfWork.SaveAsync();

        return faq;
    }
}
