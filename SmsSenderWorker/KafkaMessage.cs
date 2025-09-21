using System.Text.Json.Serialization;

namespace SmsSenderWorker;

public class KafkaSendSmsMessage
{
    public string Num { get; set; } = string.Empty;
    public string Msg { get; set; } = string.Empty;
}

[JsonSourceGenerationOptions(WriteIndented = true,PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(KafkaSendSmsMessage))]
internal partial class KafkaSendSmsMessageContext : JsonSerializerContext
{
}
