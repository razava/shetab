﻿namespace Domain.Models.Relational;

public class PollAnswer
{
    public Guid Id { get; set; }
    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    //This is used for descriptive polls
    public string? Text { get; set; }
    public IEnumerable<PollChoice> Choices { get; set; } = new List<PollChoice>();
    public DateTime DateTime { get; set; }
}




