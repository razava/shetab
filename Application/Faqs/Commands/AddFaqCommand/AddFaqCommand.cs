using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Commands.AddFaqCommand;

public sealed record AddFaqCommand(
    int InstanceId,
    string Question,
    string Answer,
    bool IsDeleted) : IRequest<Result<Faq>>;