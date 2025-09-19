using Domain.Enums;

namespace Domain.Entities;

public class Sms
{
    public long Id { get; set; }
    public Customer Sender { get; set; } = new();
    public string MessageBody { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; }
    public List<SmsSendResult> SendResults { get; set; } = [];
    public DateTime CreatedDate { get; set; }
}
