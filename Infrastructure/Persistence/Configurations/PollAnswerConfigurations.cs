using Domain.Models.Relational.PollAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PollAnswerConfigurations : IEntityTypeConfiguration<PollAnswer>
{
    public void Configure(EntityTypeBuilder<PollAnswer> builder)
    {

    }
}
