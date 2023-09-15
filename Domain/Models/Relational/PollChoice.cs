namespace Domain.Models.Relational;

public class PollChoice
{
    public int Id { get; set; }
    public string ShortTitle { get; set; } = null!;
    public string Text { get; set; } = null!;
    public int Order { get; set; }
    public IEnumerable<PollAnswer> Answers { get; set; } = new List<PollAnswer>();
}




