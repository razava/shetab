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
    }
}
