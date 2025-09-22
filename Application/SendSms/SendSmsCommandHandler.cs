using Application.Common.Interfaces;
using Cortex.Mediator.Commands;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.SendSms;

public class SendSmsCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<SendSmsCommand, long>
{
    public async Task<long> Handle(SendSmsCommand command, CancellationToken cancellationToken)
    {
        var customer = dbContext
            .Customers
            .Include(x => x.Account)
            .FirstOrDefault(x => x.Id == command.CustomerId) ??
            throw new ArgumentException("Customer not found");

        if (customer.Account.AccountBalance < command.Recievers.Count)
            throw new InvalidOperationException("You have insufficient account balance. Please Charge it");

        customer.Account.AccountBalance -= command.Recievers.Count;

        var newSms = new Sms()
            {
                MessageBody = command.SmsContent,
                Priority = command.SmsType switch
                {
                    SmsType.Normal => PriorityLevel.Normal,
                    SmsType.Express => PriorityLevel.Express,
                    _ => PriorityLevel.Normal,
                },
                SendResults = command.Recievers.Select(x => new Domain.Entities.SmsSendResult()
                {
                    RecieverNumber = x,
                    Status = SendStatus.New,
                }).ToList(),
                Sender = customer,
                CreatedDate = DateTime.UtcNow,
            };
        dbContext.Sms.Add(newSms);
            
        await dbContext.SaveChangesAsync(cancellationToken);
        return newSms.Id;
    }
}
