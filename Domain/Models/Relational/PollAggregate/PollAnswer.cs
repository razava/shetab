using Domain.Models.Relational.IdentityAggregate;

namespace Domain.Models.Relational.PollAggregate;

public class PollAnswer
{
    public Guid Id { get; set; }
    public int PollId { get; set; }
    //public Poll Poll { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    //This is used for descriptive polls
    public string? Text { get; set; }
    public IEnumerable<PollChoice> Choices { get; set; } = new List<PollChoice>();
    public DateTime DateTime { get; set; }

    private PollAnswer()
    {
        
    }

    public static PollAnswer Create(string userId, string? text, List<PollChoice> choices)
    {
        var answer = new PollAnswer()
        {
            UserId = userId,
            Text = text,
            Choices = choices
        };

        return answer;
    }
}
