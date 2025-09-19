using Application.Common.Interfaces;
using Cortex.Mediator.Commands;
using Domain.Enums;

namespace Application.SendSms;

public class SendSmsCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<SendSmsCommand, int>
{
    public Task<int> Handle(SendSmsCommand command, CancellationToken cancellationToken)
    {
        var customer = dbContext.Customers.FirstOrDefault(x => x.Id == command.CustomerId) ??
            throw new ArgumentException("Customer not found");

        if (customer.Account.AccountBalance < command.Recievers.Count)
            throw new InvalidOperationException("You have insufficient account balance. Please recharge it");

        customer.Account.AccountBalance -= command.Recievers.Count;
        
        dbContext.Sms.Add(
            new()
            {
                MessageBody = command.SmsContent,
                Priority = command.SmsType switch
                {
                    SmsType.Normal => PriorityLevel.Normal,
                    SmsType.Express => PriorityLevel.Express,
                    _ => PriorityLevel.Normal,
                },
                SendResults = command.Recievers.Select(x=> new Domain.Entities.SmsSendResult()
                {
                    RecieverNumber = x,
                    Status = SendStatus.New,
                }).ToList(),
                Sender = customer,
            });

        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
