using Application.SendSms;
using Infrastructure;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructureServices();
builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

var todosApi = app.MapGroup("/sendsms");
todosApi.MapPost("/", async (SendSmsCommand command) => await SendSms(command));

async Task SendSms(SendSmsCommand command)
{
	try
	{

		Results.Ok();
    }
	catch (Exception ex)
	{

		throw;
	}
}

app.Run();

