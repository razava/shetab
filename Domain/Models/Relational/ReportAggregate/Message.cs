using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;

namespace Domain.Models.Relational.ReportAggregate;

public class Message : BaseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public DateTime? LastSent { get; set; }
    public MessageType MessageType { get; set; }
    public Guid SubjectId { get; set; }
    public string? FromId { get; set; }
    public ApplicationUser? From { get; set; }
    public MessageRecepient Recepient { get; set; } = null!;
    public Guid? ReportId { get; set; }
    public Report? Report { get; set; }
}

public class MessageRecepient
{
    private MessageRecepient() { }
    public static MessageRecepient Create(RecepientType type, string toId)
    {
        return new MessageRecepient()
        {
            Id = Guid.NewGuid(),
            Type = type,
            ToId = toId
        };
    }
    public Guid Id { get; set; }
    public RecepientType Type { get; set; }
    public string ToId { get; set; } = null!;
}


