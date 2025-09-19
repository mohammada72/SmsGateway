using Application.Common.Interfaces;
using Cortex.Mediator.Commands;
using Microsoft.EntityFrameworkCore;

namespace Application.ChargeAccount;

public class ChargeAccountCommandHandler(IApplicationDbContext context) : ICommandHandler<ChargeAccountCommand, int>
{
    public async Task<int> Handle(ChargeAccountCommand command, CancellationToken cancellationToken)
    {
        var customer = context.Customers
            .Include(x=> x.Account)
            .FirstOrDefault(x => x.Id == command.CustomerId) ??
            throw new ArgumentException("Customer not found");

        customer.Account.AccountBalance += command.ChargeAmount;
        await context.SaveChangesAsync(cancellationToken);
        return customer.Account.AccountBalance;
    }
}
