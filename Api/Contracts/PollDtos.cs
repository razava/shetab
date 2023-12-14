using Domain.Models.Relational.Common;

namespace Api.Contracts;

public record PollCreateDto(
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceCreateDto> Choices,
    bool IsActive);

public record PollChoiceCreateDto(
    string ShortTitle, 
    string Text, 
    int Order);


public record GetPollsDto(
    int Id,
    string Title,
    PollType PollType,
    string Question,
    List<PollChoiceDto> Choices,
    PollState PollState,
    PollAnswerDto? Answer);

public record PollChoiceDto(int Id, string ShortTitle, string Text, int Order);
public record PollAnswerDto(List<int>? Choices, string? Text);


public record PollUpdateDto(
    string? Title,
    PollType? PollType,
    string? Question,
    List<PollChoiceCreateDto>? Choices,
    PollState? PollState);


public record AnswerToPollDto(
    string? Text,
    List<int> ChoicesIds);





