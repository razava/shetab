using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SatisfactionConfigurations : IEntityTypeConfiguration<Satisfaction>
{
    public void Configure(EntityTypeBuilder<Satisfaction> builder)
    {
        builder.HasOne(s => s.Actor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
