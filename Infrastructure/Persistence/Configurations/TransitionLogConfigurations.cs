using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TransitionLogConfigurations : IEntityTypeConfiguration<TransitionLog>
{
    public void Configure(EntityTypeBuilder<TransitionLog> builder)
    {
        builder.OwnsMany(tl => tl.Attachments);
    }
}
