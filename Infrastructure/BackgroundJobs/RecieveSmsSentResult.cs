using Confluent.Kafka;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs;

public class RecieveSmsSentResult(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    ILogger<RecieveSmsSentResult> logger,
    IConfiguration configuration) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Yield();
                var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
                var config = new ConsumerConfig
                {
                    GroupId = "send-sms-group",
                    BootstrapServers = configuration.GetValue<string>("KafkaServers") ?? "localhost:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                    EnableAutoOffsetStore = false,
                };

                int batchSize = 1000;
                TimeSpan consumeTimeout = TimeSpan.FromSeconds(3);

                List<ConsumeResult<long, string>> batch = [];
                using var consumer = new ConsumerBuilder<long, string>(config).Build();
                consumer.Subscribe("SmsSentResult");

                while (batch.Count < batchSize && !stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(consumeTimeout);
                    if (consumeResult != null)
                    {
                        batch.Add(consumeResult);
                    }
                    else
                    {
                        break;
                    }
                }
                if (batch.Count > 0)
                {
                    await ProcessBatch(batch);
                    CommitBatchOffsets(consumer, batch);
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }

    private async Task ProcessBatch(List<ConsumeResult<long, string>> batch)
    {
        var dbcontext = await dbContextFactory.CreateDbContextAsync();
        var smsResults = dbcontext.SmsSendResult.Where(x => batch.Select(b => b.Message.Key).Contains(x.Id));
        foreach (var smsResult in smsResults)
        {
            smsResult.Status = bool.Parse(batch.First(x => x.Message.Key == smsResult.Id).Message.Value) ? SendStatus.Succeed : SendStatus.Failed;
        }
        await ProcessCashBack(smsResults);
        await dbcontext.SaveChangesAsync();
    }

    private static async Task ProcessCashBack(IQueryable<Domain.Entities.SmsSendResult> smsResults)
    {
        var refundData = await smsResults
                                        .Where(x => x.Status == SendStatus.Failed)
                                        .GroupBy(x => new { SmsId = x.Sms.Id, SenderId = x.Sms.Sender.Id })
                                        .Select(g => new
                                        {
                                            SmsId = g.Key.SmsId,
                                            CustomerId = g.Key.SenderId,
                                            FailureCount = g.Count(),
                                            Customer = g.First().Sms.Sender
                                        })
                                        .ToListAsync();
        if (refundData.Count == 0) return;
        var customerRefunds = refundData
                                .GroupBy(x => x.CustomerId)
                                .Select(g => new
                                {
                                    Customer = g.First().Customer,
                                    TotalRefundAmount = g.Sum(x => x.FailureCount)
                                })
                                .ToList();

        foreach (var refund in customerRefunds)
        {
            refund.Customer.Account.AccountBalance += refund.TotalRefundAmount;
        }
    }

    static void CommitBatchOffsets(IConsumer<long, string> consumer, List<ConsumeResult<long, string>> batch)
    {
        var lastOffset = batch[^1].Offset;
        var topicPartition = batch[^1].TopicPartition;

        consumer.Commit([new(topicPartition, lastOffset + 1)]);
    }
}
