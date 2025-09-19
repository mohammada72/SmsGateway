using Domain.Enums;

namespace Domain.Entities;

public class SmsSendResult
{
    public long Id { get; set; }
    public string RecieverNumber { get; set; } = string.Empty;
    public SendStatus Status { get; set; }
    public DateTime SendDate { get; set; }
    public virtual Sms Sms { get; set; } = new();
}
