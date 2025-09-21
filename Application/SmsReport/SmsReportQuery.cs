using Application.Common.Models;
using Cortex.Mediator.Queries;
using System.Diagnostics.CodeAnalysis;

namespace Application.SmsReport;

public class SmsReportQuery : IQuery<PaginatedList<SmsReportModel>>
{
    public long SmsId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
