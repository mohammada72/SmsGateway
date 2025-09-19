using Application;
using Application.ChargeAccount;
using Application.Common.Models;
using Application.CreateCustomer;
using Application.SendSms;
using Application.SmsReport;
using Cortex.Mediator;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

var smsApi = app.MapGroup("/sms");
smsApi.MapPost("/", async (SendSmsCommand command, [FromServices] IMediator mediator) => await SendSms(command, mediator));
smsApi.MapGet("/", async (SmsReportQuery query, [FromServices] IMediator mediator) => await GetSmsReport(query, mediator));

var customerApi = app.MapGroup("/customer");
smsApi.MapPost("/chargeaccount", async (ChargeAccountCommand command, [FromServices] IMediator mediator) => await ChargeAccount(command, mediator));
smsApi.MapPost("/create", async (CreateCustomerCommand command, [FromServices] IMediator mediator) => await CreateCustomer(command, mediator));



async static Task SendSms(SendSmsCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<SendSmsCommand, int>(command);
        Results.Ok($"sms sent successfully with Id: {result}");
    }
    catch (Exception ex)
    {

        throw;
    }
}
async static Task CreateCustomer(CreateCustomerCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<CreateCustomerCommand, Customer>(command);
        Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}
async static Task ChargeAccount(ChargeAccountCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<ChargeAccountCommand, int>(command);
        Results.Ok($"New balance is : {result}");
    }
    catch (Exception ex)
    {

        throw;
    }
}
async static Task GetSmsReport(SmsReportQuery query, IMediator mediator)
{
    try
    {
        var result = await mediator.SendQueryAsync<SmsReportQuery, PaginatedList<SmsReportModel>>(query);
        Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}

app.Run();

