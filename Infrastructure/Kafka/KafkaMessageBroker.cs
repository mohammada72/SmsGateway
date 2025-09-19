using Application.Common.Interfaces;
using Application.Common.Models;
using Confluent.Kafka;

namespace Infrastructure.Kafka;

public class KafkaMessageBroker : IMessgeBroker<KafkaMessage>
{
    private readonly ProducerConfig _producerConfig = new()
    {
        BootstrapServers = "http://localhost:9092/",
        EnableIdempotence = true,
        Acks = Acks.All,
        MaxInFlight = 5
    };

    public Task<KafkaMessage> Consume()
    {
        throw new NotImplementedException();
    }

    public async Task Publish(KafkaMessage message)
    {
        using var producer = new ProducerBuilder<long, string>(_producerConfig).Build();
        await producer.ProduceAsync(message.TopicName, new Message<long, string>() { Key = message.Id, Value = message.Message });
    }

    public async Task Publish(IEnumerable<KafkaMessage> messages)
    {
        using var producer = new ProducerBuilder<long, string>(_producerConfig).Build();
        producer.BeginTransaction();
        foreach (var message in messages)
        {
            await producer.ProduceAsync(message.TopicName, new Message<long, string>() { Key = message.Id, Value = message.Message });
        }
        producer.CommitTransaction();
    }

}
