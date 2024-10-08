﻿using Application.Polls.Common;
using Domain.Models.Relational.Common;

namespace Application.Polls.Commands.UpdatePoll;

public record UpdatePollCommand(
    int Id,
    string? Title,
    PollType? PollType,
    string? Question,
    List<PollChoiceRequest>? Choices,
    PollState? PollState,
    bool? isDeleted = false) : IRequest<Result<bool>>;
