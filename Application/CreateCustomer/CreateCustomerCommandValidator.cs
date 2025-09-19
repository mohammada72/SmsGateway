using FluentValidation;

namespace Application.CreateCustomer;
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{

    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.NationalId).NotEmpty().MaximumLength(11);
    }
}

