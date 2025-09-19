using AutoMapper;
using Domain.Entities;

namespace Application.SmsReport;

public class SmsReportMapper : Profile
{
    public SmsReportMapper()
    {
         CreateMap<Sms, SmsReportQuery>();
    }
}
