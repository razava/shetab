using Application.Polls.Common;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Polls.Commands.AddPollCommand;

public record AddPollCommand(
    int InstanceId,
    string UserId,
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceRequest> Choices,
    bool IsActive,
    List<IFormFile> Medias) : IRequest<Poll>;
