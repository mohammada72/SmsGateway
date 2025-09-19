using FluentValidation;

namespace Application.ChargeAccount;
public class ChargeAccountCommandValidator : AbstractValidator<ChargeAccountCommand>
{

    public ChargeAccountCommandValidator()
    {
        RuleFor(x => x.ChargeAmount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.CustomerId).NotEmpty().GreaterThan(0);
    }
}

