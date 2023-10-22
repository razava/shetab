using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ApplicationLinkConfiguration : IEntityTypeConfiguration<ApplicationLink>
{
    public void Configure(EntityTypeBuilder<ApplicationLink> builder)
    {
        builder.OwnsOne(r => r.Image);
    }
}
