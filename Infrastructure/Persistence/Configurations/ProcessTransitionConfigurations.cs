using Domain.Models.Relational;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProcessTransitionConfigurations : IEntityTypeConfiguration<ProcessTransition>
{
    public void Configure(EntityTypeBuilder<ProcessTransition> builder)
    {
        builder.HasOne(c => c.From)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.To)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

    }
}
