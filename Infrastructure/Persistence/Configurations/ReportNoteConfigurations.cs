using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReportNoteConfigurations : IEntityTypeConfiguration<ReportNote>
{
    public void Configure(EntityTypeBuilder<ReportNote> builder)
    {
        builder.HasOne<Report>().WithMany();
        builder.HasOne<ApplicationUser>().WithMany();
    }
}
