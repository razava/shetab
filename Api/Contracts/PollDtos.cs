﻿using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record PollCreateDto(
    [Required] [MaxLength(256)]
    string Title,
    [Required]
    PollType PollType,
    [Required] [MaxLength(5120)]
    string Question,
    [Required]
    List<PollChoiceCreateDto> Choices,
    [Required]
    bool IsActive);

public record PollChoiceCreateDto(
    [Required] [MaxLength(64)]
    string ShortTitle,
    [Required] [MaxLength(5120)]
    string Text,
    [Required]
    int Order);


public record GetPollsDto(
    int Id,
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceDto> Choices,
    PollState PollState,
    PollAnswerDto? Answer,
    bool IsDeleted);

public record PollChoiceDto(int Id, string ShortTitle, string Text, int Order);
public record PollAnswerDto(List<int>? Choices, string? Text);


public record PollUpdateDto(
    [MaxLength(256)]
    string? Title,
    PollType? PollType,
    [MaxLength(5120)]
    string? Question,
    List<PollChoiceCreateDto>? Choices,
    PollState? PollState,
    bool? isDeleted = false);


public record AnswerToPollDto(
    [MaxLength(5120)]   //todo : review
    string? Text,
    List<int> ChoicesIds);


public record PollChoiceResultDto(string ShortTitle, double Percentage);
public record PollResultDto(long Count, List<PollChoiceResultDto> PollChoicesResults);



