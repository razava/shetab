using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Commands.UpdateFaqCommand;

public sealed record UpdateFaqCommand(
    int Id,
    string? Question,
    string? Answer,
    bool? IsDeleted) : IRequest<Result<Faq>>;