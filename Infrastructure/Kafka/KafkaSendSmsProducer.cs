using Application.Common.Interfaces;
using Application.Common.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Kafka;

public class KafkaSendSmsProducer(IConfiguration configuration) : IKafkaMessgeProducer<long, KafkaSendSmsMessage>
{
    private readonly string _kafkaServers = configuration.GetValue<string>("KafkaServers") ?? "localhost:9092";
    public async Task Publish(KeyValuePair<long, KafkaSendSmsMessage> message, string topicName)
    {

        var config = new ProducerConfig()
        {
            BootstrapServers = _kafkaServers,
            EnableIdempotence = true,
            Acks = Acks.All,
            MaxInFlight = 5
        };
        using var producer = new ProducerBuilder<long, string>(config).Build();
        await producer.ProduceAsync(topicName, 
            new Message<long, string>() { 
                Key = message.Key, 
                Value = System.Text.Json.JsonSerializer.Serialize(message.Value) });
    }

    public async Task Publish(IEnumerable<KeyValuePair<long, KafkaSendSmsMessage>> messages, string topicName)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = _kafkaServers,
            EnableIdempotence = true,
            Acks = Acks.All,
            MaxInFlight = 5,
            TransactionalId = Guid.NewGuid().ToString()
        };

        using var producer = new ProducerBuilder<long, string>(config).Build();
        producer.InitTransactions(TimeSpan.FromSeconds(10));
        producer.BeginTransaction();
        try
        {
            foreach (var message in messages)
            {
                await producer.ProduceAsync(topicName, 
                    new Message<long, string>() { 
                        Key = message.Key, 
                        Value = System.Text.Json.JsonSerializer.Serialize(message.Value) });
            }
            producer.CommitTransaction();
        }
        catch (Exception)
        {
            producer.AbortTransaction();
            throw;
        }
    }

}
