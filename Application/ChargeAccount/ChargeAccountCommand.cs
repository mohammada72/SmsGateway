using Cortex.Mediator.Commands;

namespace Application.ChargeAccount;

public class ChargeAccountCommand : ICommand<int>
{
    public long CustomerId { get; set; }
    public int ChargeAmount { get; set; }
}
