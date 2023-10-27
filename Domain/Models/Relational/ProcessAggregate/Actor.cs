using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models.Relational.ProcessAggregate;

[Index(nameof(Identifier), IsUnique = true)]
public class Actor
{
    public int Id { get; set; }
    public string Identifier { get; set; } = null!;
    public ActorType Type { get; set; }
    public string? BotActorId { get; set; }
    public BotActor? BotActor { get; set; }
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<Region> Regions { get; set; } = new List<Region>();
    public ICollection<ProcessStage> Stages { get; set; } = new List<ProcessStage>();
}
