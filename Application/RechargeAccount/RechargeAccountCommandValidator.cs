using FluentValidation;

namespace Application.RechargeAccount;
public class RechargeAccountCommandValidator : AbstractValidator<RechargeAccountCommand>
{

    public RechargeAccountCommandValidator()
    {
        RuleFor(x => x.RechargeAmount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.CustomerId).NotEmpty().GreaterThan(0);
    }
}

