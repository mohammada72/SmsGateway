using Domain.Entities;
using Domain.Enums;

namespace Application.SmsReport;

public class SmsReportModel
{
    public long Id { get; set; }
    public Customer Sender { get; set; } = new();
    public string MessageBody { get; set; } = string.Empty;
    public List<SendResult> SendResults { get; set; } = [];
}

public class SendResult
{
    public string RecieverNumber { get; set; } = string.Empty;
    public SendStatus Status { get; set; }
    public DateTime SendTime { get; set; }
}

