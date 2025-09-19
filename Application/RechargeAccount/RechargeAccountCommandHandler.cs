using Application.Common.Interfaces;
using Cortex.Mediator.Commands;
using Microsoft.EntityFrameworkCore;

namespace Application.RechargeAccount;

public class RechargeAccountCommandHandler(IApplicationDbContext context) : ICommandHandler<RechargeAccountCommand, int>
{
    public async Task<int> Handle(RechargeAccountCommand command, CancellationToken cancellationToken)
    {
        var customer = context.Customers
            .Include(x=> x.Account)
            .FirstOrDefault(x => x.Id == command.CustomerId) ??
            throw new ArgumentException("Customer not found");

        customer.Account.AccountBalance += command.RechargeAmount;
        await context.SaveChangesAsync(cancellationToken);
        return customer.Account.AccountBalance;
    }
}
