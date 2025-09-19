using Application.Common.Interfaces;
using Cortex.Mediator.Commands;
using Domain.Entities;

namespace Application.CreateCustomer;

public class CreateCustomerCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateCustomerCommand, Customer>
{
    public async Task<Customer> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = command.Name,
            Email = command.Email,
            NationalId = command.NationalId,
            Account = new() { AccountBalance = 0 }
        };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return customer;
    }
}
