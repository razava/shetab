using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MessageConfigurations : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(m => m.Report)
            .WithMany(r => r.Messages)
            .HasForeignKey(m => m.ReportId);

        builder.OwnsOne(m => m.Recepient);
    }
}
