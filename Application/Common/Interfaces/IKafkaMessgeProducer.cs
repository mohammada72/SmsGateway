namespace Application.Common.Interfaces;

public interface IKafkaMessgeProducer<TKey,TValue>
{
    Task Publish(IEnumerable<KeyValuePair<TKey,TValue>> messages, string topicName);
    Task Publish(KeyValuePair<TKey,TValue> message, string topicName);
}
