﻿using Domain.Models.Relational.Common;
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
    public ICollection<MessageRecepient> Recepients { get; set; } = new List<MessageRecepient>();
    public Guid? ReportId { get; set; }
}

public class MessageRecepient
{
    public Guid Id { get; set; }
    public RecepientType Type { get; set; }
    public string ToId { get; set; } = null!;
}


