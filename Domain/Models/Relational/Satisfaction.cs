using Domain.Models.Relational.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Relational;

public class Satisfaction : Entity
{
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
    public int Rating { get; set; }
    public string Comments { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string ActorId { get; set; } = null!;
    public ApplicationUser Actor { get; set; } = null!;

    private Satisfaction(Guid id) : base(id) { }

    public static Satisfaction Create(Guid reportId, string userId, string Comments, int rating)
    {
        return new Satisfaction(Guid.NewGuid())
        {
            ReportId = reportId,
            ActorId = userId,
            Comments = Comments,
            Rating = rating,
            DateTime = DateTime.UtcNow,
        };
    }

    public void Update(string userId, string comments, int rating)
    {
        Comments = $"{comments}\r\n##########\r\n{DateTime}#{ActorId}#{Rating}\r\n{Comments}";
        Rating = rating;
        DateTime = DateTime.UtcNow;
        ActorId = userId;
    }
}
