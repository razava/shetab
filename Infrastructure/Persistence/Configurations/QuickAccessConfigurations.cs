using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class QuickAccessConfigurations : IEntityTypeConfiguration<QuickAccess>
{
    public void Configure(EntityTypeBuilder<QuickAccess> builder)
    {
        builder.HasOne(c => c.ShahrbinInstance)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(q => q.Media);

    }
}
