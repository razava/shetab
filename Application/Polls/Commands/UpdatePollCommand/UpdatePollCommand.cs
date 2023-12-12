using Application.Polls.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Polls.Commands.UpdatePollCommand;

public record UpdatePollCommand(
    int Id,
    string? Title,
    PollType? PollType,
    string? Question,
    List<PollChoiceRequest>? Choices,
    PollState? PollState,
    List<IFormFile>? Medias) : IRequest<Poll>;
