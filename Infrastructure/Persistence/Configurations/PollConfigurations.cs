using Domain.Models.Relational.PollAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PollConfigurations : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasMany(p => p.Answers).WithOne().OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasMany(p => p.Choices).WithOne().OnDelete(DeleteBehavior.ClientSetNull);
    }
}
