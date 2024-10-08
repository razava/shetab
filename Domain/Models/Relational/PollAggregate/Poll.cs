﻿using Domain.Exceptions;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;

namespace Domain.Models.Relational.PollAggregate;

public class Poll : BaseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public PollType PollType { get; set; }
    public string Question { get; set; } = null!;
    public IEnumerable<PollChoice> Choices { get; set; } = new List<PollChoice>();
    public DateTime Created { get; set; }
    public DateTime? Expiration { get; set; }
    public bool IsDeleted { get; set; }
    public PollState Status { get; set; }
    public List<PollAnswer> Answers { get; set; } = new List<PollAnswer>();
    //public List<Media> PollMedias { get; set; } = new List<Media>();
    public string AuthorId { get; set; } = null!;
    public ApplicationUser Author { get; set; } = null!;

    private Poll() { }

    public static Poll Create(
        int instanceId,
        string authorId,
        string title,
        PollType pollType,
        string question,
        List<PollChoice> choices,
        bool isDeleted)
    {
        var poll = new Poll()
        {
            ShahrbinInstanceId = instanceId,
            AuthorId = authorId,
            Title = title,
            PollType = pollType,
            Question = question,
            Choices = choices,
            IsDeleted = isDeleted,
            Created = DateTime.UtcNow
        };

        return poll;
    }

    public void Update(
        string? title,
        PollType? pollType,
        string? question,
        List<PollChoice>? choices,
        PollState? status,
        bool? isDeleted = false)
    {
        Title = title ?? Title;
        PollType = pollType ?? PollType;
        Question = question ?? Question;
        Choices = choices ?? Choices;
        Status = status ?? Status;
        IsDeleted = isDeleted ?? IsDeleted;
    }

    public void Answer(string userId, List<int> choices, string? text)
    {
        if (Answers.Any(a => a.UserId == userId))
        {
            throw new ParticipationMoreThanOnceException();
        }
        if (PollType == PollType.Descriptive)
        {
            if (text is null)
                throw new NullAnswerTextException();
                //throw new Exception("Text cannot be null in descriptive polls.");
            Answers.Add(PollAnswer.Create(userId, text, new List<PollChoice>()));
        }
        if (PollType == PollType.SingleChoice)
        {
            if (choices.Count != 1)
                throw new OneChoiceAnswerLimitException();
                //throw new Exception("Single choice polls must have exactly 1 answer.");
            var choice = Choices.Where(c => choices.Contains(c.Id)).ToList();
            if (choice is null || choice.Count != 1)
            {
                throw new InvalidChoiceException();
                //throw new Exception("Invalid choice!");
            }
            Answers.Add(PollAnswer.Create(userId, null, choice));
        }
        if (PollType == PollType.MultipleChoice)
        {
            var choice = Choices.Where(c => choices.Contains(c.Id)).ToList();
            if (choice is null || choice.Count == 0)
            {
                throw new InvalidChoiceException();
                //throw new Exception("Invalid choice!");
            }
            Answers.Add(PollAnswer.Create(userId, null, choice));
        }
    }
}




