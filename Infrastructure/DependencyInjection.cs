using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.BackgroundJobs;
using Infrastructure.Data;
using Infrastructure.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;
public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("SmsGatewayDb");

        builder.Services.AddDbContextPool<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connectionString,
                providerOptions => { providerOptions.EnableRetryOnFailure(); });
        });

        builder.Services.AddPooledDbContextFactory<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                connectionString,
                providerOptions => { providerOptions.EnableRetryOnFailure(); });
        });

        builder.EnrichSqlServerDbContext<ApplicationDbContext>();

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddSingleton<IKafkaMessgeProducer<long, KafkaSendSmsMessage>, KafkaSendSmsProducer>();

        builder.Services.AddHostedService<SendSmsToKafka>();
        builder.Services.AddHostedService<RecieveSmsSentResult>();
        builder.Services.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });
    }
}
