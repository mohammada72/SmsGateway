using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Account);

        builder.Property(x => x.Email)
            .HasColumnType("nvarchar(255)")
            .HasMaxLength(300)
            .HasConversion(
                v => v.ToString(),
                w => new Email(w));

        builder.Property(x => x.NationalId)
            .HasColumnType("nvarchar(11)")
            .HasMaxLength(11)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                w => new NationalId(w));
    }
}
