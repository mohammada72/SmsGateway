using Cortex.Mediator.Commands;
using Domain.Entities;

namespace Application.CreateCustomer;

public class CreateCustomerCommand : ICommand<Customer>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
}
