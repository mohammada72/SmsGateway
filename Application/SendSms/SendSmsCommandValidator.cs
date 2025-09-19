using FluentValidation;

namespace Application.SendSms;
public class SendSmsCommandValidator : AbstractValidator<SendSmsCommand>
{
    public SendSmsCommandValidator()
    {
        RuleFor(x => x.SmsContent).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Recievers).NotEmpty().Must(x=> x.Count > 0);
    }
}

