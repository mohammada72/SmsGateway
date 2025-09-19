using Cortex.Mediator.Commands;

namespace Application.RechargeAccount;

public class RechargeAccountCommand : ICommand<int>
{
    public long CustomerId { get; set; }
    public int RechargeAmount { get; set; }
}
