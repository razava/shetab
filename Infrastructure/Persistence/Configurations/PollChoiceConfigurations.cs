using Domain.Models.Relational.PollAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PollChoiceConfigurations : IEntityTypeConfiguration<PollChoice>
{
    public void Configure(EntityTypeBuilder<PollChoice> builder)
    {
        builder.HasOne<Poll>().WithMany(p => p.Choices).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasMany(pc => pc.Answers).WithMany(pc => pc.Choices);
    }
}
