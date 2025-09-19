using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Application.BackgroundJobs;

public class SendSmsToKafka(IApplicationDbContext dbContext, IMessgeBroker<KafkaMessage> messgeBroker) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();
            var messages = await dbContext.SmsSendResult
                        .FromSql($"SELECT * FROM SmsSendResult WITH (ROWLOCK, READPAST, UPDLOCK)")
                        .Include(x => x.Sms)
                        .Where(x => x.Status == Domain.Enums.SendStatus.New && x.Sms.Priority == Domain.Enums.PriorityLevel.Express)
                        .OrderBy(x => x.Id)
                        .Take(10000)
                        .ToListAsync(cancellationToken: stoppingToken);

            if (messages.Count == 0)
            {
                messages = await dbContext.SmsSendResult
                        .FromSql($"SELECT * FROM SmsSendResult WITH (ROWLOCK, READPAST, UPDLOCK)")
                        .Include(x => x.Sms)
                        .Where(x => x.Status == Domain.Enums.SendStatus.New && x.Sms.Priority == Domain.Enums.PriorityLevel.Normal)
                        .OrderBy(x => x.Id)
                        .Take(10000)
                        .ToListAsync(cancellationToken: stoppingToken);
            }

            if (messages.Count == 0)
            {
                await Task.Delay(100, stoppingToken);
                continue;
            }

            await messgeBroker.Publish(messages.Select(x => new KafkaMessage()
            {
                Id = x.Id,
                Message = x.Sms.MessageBody,
                TopicName = $"{x.Sms.Priority}SmsSend"
            }));

            messages.ForEach(x => x.Status = Domain.Enums.SendStatus.SentToQueue);
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
