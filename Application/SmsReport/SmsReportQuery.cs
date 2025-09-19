using Application.Common.Models;
using Cortex.Mediator.Queries;

namespace Application.SmsReport;

public class SmsReportQuery : IQuery<PaginatedList<SmsReportModel>>
{
    public long SmsId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
