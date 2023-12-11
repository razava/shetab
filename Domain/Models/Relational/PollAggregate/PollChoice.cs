namespace Domain.Models.Relational.PollAggregate;

public class PollChoice
{
    public int Id { get; set; }
    public string ShortTitle { get; set; } = null!;
    public string Text { get; set; } = null!;
    public int Order { get; set; }
    public IEnumerable<PollAnswer> Answers { get; set; } = new List<PollAnswer>();

    private PollChoice()
    {
        
    }

    public static PollChoice Create(string shortTitle, string text, int order)
    {
        var pollChoice = new PollChoice()
        {
            ShortTitle = shortTitle,
            Text = text,
            Order = order
        };

        return pollChoice;
    }
}
