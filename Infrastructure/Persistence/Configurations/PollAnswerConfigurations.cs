using Domain.Models.Relational.PollAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PollAnswerConfigurations : IEntityTypeConfiguration<PollAnswer>
{
    public void Configure(EntityTypeBuilder<PollAnswer> builder)
    {
        builder.HasOne<Poll>().WithMany(p => p.Answers).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasMany(pa => pa.Choices).WithMany(pc => pc.Answers);
        builder.HasOne(pa => pa.User).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
    }
}
