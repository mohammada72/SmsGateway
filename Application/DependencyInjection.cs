using Application.Common.Interfaces;
using Application.Common.Models;
using Application.SmsReport;
using AutoMapper;
using Cortex.Mediator.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    { 
        builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(SmsReportMapper).Assembly));
        
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddCortexMediator(builder.Configuration,
                                        [typeof(IApplicationDbContext)],
                                        options => options.AddDefaultBehaviors());

    }
}
