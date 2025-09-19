using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class SmsConfiguration : IEntityTypeConfiguration<Sms>
{
    public void Configure(EntityTypeBuilder<Sms> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Navigation(x => x.Sender);
        
        builder
            .OwnsMany(x => x.SendResults);
            
    }
}
