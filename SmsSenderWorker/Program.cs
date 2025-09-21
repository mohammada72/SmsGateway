using Confluent.Kafka;
using SmsSenderWorker.ExternalServices;
using System.CommandLine;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace SmsSenderWorker;

public class Program
{
    static void Main(string[] args)
    {
        RootCommand rootCommand = new("Worker for sending sms fast");
        rootCommand.Options.Add(new Option<string>("--servers"));
        rootCommand.Options.Add(new Option<string>("--topic-name"));
        rootCommand.Options.Add(new Option<string>("--group-id"));
        rootCommand.Options.Add(new Option<string>("--worker-name"));

        ParseResult parseResult = rootCommand.Parse(args);
        var servers = parseResult.GetValue<string>("--servers");
        var topicName = parseResult.GetValue<string>("--topic-name");
        var groupId = parseResult.GetValue<string>("--group-id");
        var workerName = parseResult.GetValue<string>("--worker-name");

        var consumerConfig = new ConsumerConfig
        {
            GroupId = groupId,
            BootstrapServers = servers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            ClientId = workerName,
            EnableAutoCommit = false,
            EnableAutoOffsetStore = false,
        };

        var producerConfig = new ProducerConfig()
        {
            BootstrapServers = servers,
            EnableIdempotence = true,
            Acks = Acks.All,
        };

        while (true)
        {
            try
            {
                using var consumer = new ConsumerBuilder<long, string>(consumerConfig).Build();
                consumer.Subscribe(topicName);
                var message = consumer.Consume();

                var deserilizedMessage = JsonSerializer.Deserialize(message.Message.Value, KafkaSendSmsMessageContext.Default.KafkaSendSmsMessage);
                if (deserilizedMessage != null)
                {
                    using var producer = new ProducerBuilder<long, string>(producerConfig).Build();
                    if (SmsSenderService.SendSms(deserilizedMessage.Msg, deserilizedMessage.Num).Result)
                    {
                        producer.Produce("SmsSentResult", new Message<long, string>() { Key = message.Message.Key, Value = "true" });
                    }
                    else
                    {
                        producer.Produce("SmsSentResult", new Message<long, string>() { Key = message.Message.Key, Value = "false" });
                    }
                }
                consumer.Commit(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
