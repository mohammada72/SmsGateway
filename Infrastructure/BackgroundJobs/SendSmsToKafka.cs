using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs;

public class SendSmsToKafka(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    IKafkaMessgeProducer<long, KafkaSendSmsMessage> messgeBroker,
    ILogger<SendSmsToKafka> logger) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Yield();
                var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
                var messages = await dbContext.SmsSendResult
                            .FromSql($"SELECT * FROM SmsSendResult WITH (ROWLOCK, READPAST, UPDLOCK)")
                            .Include(x => x.Sms)
                            .Where(x => x.Status == Domain.Enums.SendStatus.New)
                            .OrderByDescending(x => x.Sms.Priority) 
                            .ThenBy(x => x.Id)
                            .Take(1000)
                            .ToListAsync(cancellationToken: stoppingToken);

                if (messages.Count == 0)
                {
                    await Task.Delay(100, stoppingToken);
                    continue;
                }

                await messgeBroker.Publish(
                    messages.Select(x =>
                        new KeyValuePair<long, KafkaSendSmsMessage>(
                            x.Id,
                            new KafkaSendSmsMessage { Msg = x.Sms.MessageBody, Num = x.RecieverNumber })),
                    $"{messages.First().Sms.Priority}SmsSend");

                messages.ForEach(x => x.Status = Domain.Enums.SendStatus.SentToQueue);
                await dbContext.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
