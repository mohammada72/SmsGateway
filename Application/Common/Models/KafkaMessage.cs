namespace Application.Common.Models
{
    public class KafkaMessage
    {
        public string TopicName { get; set; } = string.Empty;
        public long Id { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
