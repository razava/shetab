using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReportConfigurations : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasOne(report => report.Feedback)
            .WithOne(feedback => feedback.Report)
            .HasForeignKey<Feedback>(feedback => feedback.ReportId);

        builder.HasOne(report => report.Process)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.ShahrbinInstance)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(r => r.Address);
        builder.OwnsMany(r => r.Medias);

        builder.HasOne(report => report.Satisfaction)
            .WithOne(s => s.Report)
            .IsRequired(false);
    }
}
