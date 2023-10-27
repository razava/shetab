using Domain.Models.Relational;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BotActorConfigurations : IEntityTypeConfiguration<BotActor>
{
    public void Configure(EntityTypeBuilder<BotActor> builder)
    {
        builder.HasOne(ba => ba.Actor)
            .WithOne(a => a.BotActor);
        builder.HasMany(ba => ba.DestinationActors)
            .WithMany()
            .UsingEntity("BotActorDestinationActors");
    }
}
