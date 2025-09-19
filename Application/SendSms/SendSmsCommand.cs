using Cortex.Mediator.Commands;

namespace Application.SendSms;

public record SendSmsCommand
    (
    long CustomerId,
    string SmsContent,
    List<string> Recievers,
    SmsType SmsType
    ) : ICommand<int>;
