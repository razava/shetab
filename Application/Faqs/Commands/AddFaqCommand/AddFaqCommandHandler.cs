﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Commands.AddFaqCommand;

internal sealed class AddFaqCommandHandler(
    IFaqRepository faqRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddFaqCommand, Result<Faq>>
{
    
    public async Task<Result<Faq>> Handle(AddFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = Faq.Create(request.InstanceId, request.Question, request.Answer, request.IsDeleted);

        faqRepository.Insert(faq);
        await unitOfWork.SaveAsync();

        return faq;
    }
}
