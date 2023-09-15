namespace Domain.Models.Relational;

public class Poll : BaseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public PollType PollType { get; set; }
    public string Question { get; set; } = null!;
    public IEnumerable<PollChoice> Choices { get; set; } = new List<PollChoice>();
    public DateTime Created { get; set; }
    public DateTime? Expiration { get; set; }
    public PollState Status { get; set; }
    public List<PollAnswer> Answers { get; set; } = new List<PollAnswer>();
    public List<Media> PollMedias { get; set; } = new List<Media>();
    public string AuthorId { get; set; } = null!;
    public ApplicationUser Author { get; set; } = null!;
}




