using Application.ChargeAccount;
using Application.Common.Models;
using Application.CreateCustomer;
using Application.SendSms;
using Application.SmsReport;
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

var smsApi = app.MapGroup("/sms");
smsApi.MapPost("/", async ([FromBody] SendSmsCommand command, [FromServices] IMediator mediator) => await SendSms(command, mediator));
smsApi.MapGet("/", async (long smsId, int pageSize, int pageNumber, [FromServices] IMediator mediator) => await GetSmsReport(smsId, pageSize, pageNumber, mediator));

var customerApi = app.MapGroup("/customer");
customerApi.MapPost("/chargeaccount", async ([FromBody] ChargeAccountCommand command, [FromServices] IMediator mediator) => await ChargeAccount(command, mediator));
customerApi.MapPost("/create", async ([FromBody] CreateCustomerCommand command, [FromServices] IMediator mediator) => await CreateCustomer(command, mediator));



async static Task<IResult> SendSms(SendSmsCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<SendSmsCommand, long>(command);
        return Results.Ok($"sms sent successfully with Id: {result}");
    }
    catch (Exception ex)
    {

        throw;
    }
}
async static Task<IResult> CreateCustomer(CreateCustomerCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<CreateCustomerCommand, Customer>(command);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}
async static Task<IResult> ChargeAccount(ChargeAccountCommand command, IMediator mediator)
{
    try
    {
        var result = await mediator.SendCommandAsync<ChargeAccountCommand, int>(command);
        return Results.Ok($"New balance is : {result}");
    }
    catch (Exception ex)
    {

        throw;
    }
}
async static Task<IResult> GetSmsReport(long smsId, int pageSize, int pageNumber, IMediator mediator)
{
    try
    {
        var result = await mediator.SendQueryAsync<SmsReportQuery, PaginatedList<SmsReportModel>>(new()
        {
            SmsId = smsId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Results.Ok(result);
    }
    catch (Exception ex)
    {

        throw;
    }
}

app.Run();

