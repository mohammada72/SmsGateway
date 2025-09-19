using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Sms> Sms { get; }
    DbSet<SmsSendResult> SmsSendResult { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
