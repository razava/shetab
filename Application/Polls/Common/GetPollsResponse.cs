﻿using Domain.Models.Relational.Common;

namespace Application.Polls.Common;

public record GetPollsResponse(
    int Id,
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceResponse> Choices,
    PollState PollState,
    DateTime Creatted,
    DateTime? Expiration,
    PollAnswerResponse? Answer,
    bool IsDeleted);

public record PollChoiceResponse(int Id, string ShortTitle, string Text, int Order);
public record PollAnswerResponse(List<PollAnswerItemResponse>? Choices, string? Text);
public record PollAnswerItemResponse(int Id);