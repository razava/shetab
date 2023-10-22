using Domain.Models.Relational;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FeedbackConfigurations : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasOne(c => c.ShahrbinInstance)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(f => f.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
