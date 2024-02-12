using Domain.Models.Relational.IdentityAggregate;
using Domain.Primitives;

namespace Domain.Models.Relational.ReportAggregate;

public class ReportNote : Entity
{
    private ReportNote(Guid id) : base(id)
    {

    }

    public Guid ReportId { get; private set; }
    public string UserId { get; private set; } = null!;
    public string Text { get; private set; } = string.Empty;
    public DateTime Created { get; private set; }
    public DateTime Updated { get; private set; }
    public bool IsDeleted { get; private set; }

    public static ReportNote Create(string userId, Guid reportId, string text)
    {
        return new ReportNote(Guid.NewGuid())
        {
            UserId = userId,
            ReportId = reportId,
            Text = text,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            IsDeleted = false
        };
    }

    public void Update(string text)
    {
        Text = text;
        Updated = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        Updated = DateTime.UtcNow;
    }
}
