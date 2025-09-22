using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cortex.Mediator.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.SmsReport;

public class SmsReportQueryHandler(IApplicationDbContext dbContext, IMapper mapper) : IQueryHandler<SmsReportQuery, PaginatedList<SmsReportModel>>
{
    public Task<PaginatedList<SmsReportModel>> Handle(SmsReportQuery query, CancellationToken cancellationToken)
    {
        var result = dbContext.Sms.Include(x=> x.SendResults)
            .AsNoTrackingWithIdentityResolution()
            .Where(x => x.Id == query.SmsId)
            .ProjectTo<SmsReportModel>(mapper.ConfigurationProvider);

        return PaginatedList<SmsReportModel>.CreateAsync(result, query.PageNumber, query.PageSize, cancellationToken);
    }
}
