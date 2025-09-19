using Application;
using Application.CreateCustomer;
using Application.SendSms;
using Cortex.Mediator;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructureServices();
builder.AddApplicationServices();
builder.Services.AddOpenApiDocument();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}
app.MapDefaultEndpoints();

var api = app.MapGroup("/sendsms");
api.MapPost("/", async (SendSmsCommand command,[FromServices] IMediator mediator) => await SendSms(command, mediator));

async Task SendSms(SendSmsCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<SendSmsCommand,int>(command);
        Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}
async Task CreateCustomer(CreateCustomerCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<CreateCustomerCommand,Customer>(command);
        Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}
async Task ChargeAccount(SendSmsCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<SendSmsCommand,int>(command);
        Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}
app.Run();

