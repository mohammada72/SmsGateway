var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SmsGateway>("smsgateway");

builder.Build().Run();
