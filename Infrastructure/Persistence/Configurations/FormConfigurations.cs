using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FormConfigurations : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {
        builder.HasMany(f => f.Categories)
            .WithOne(c => c.Form)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsMany(f => f.Elements);
    }
}
