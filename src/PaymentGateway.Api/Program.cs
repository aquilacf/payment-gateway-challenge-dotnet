using PaymentGateway.Api;
using PaymentGateway.Application;
using PaymentGateway.Infrastructure;
using PaymentGateway.ServiceDefaults;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfratructure();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapEndpoints();

app.Run();