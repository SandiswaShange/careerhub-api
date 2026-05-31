using API.Data;
using Scalar.AspNetCore;
using API.Middleware;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();//assignemnt 4.3

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Controllers
builder.Services.AddControllers();

builder.Services.AddProblemDetails(); // enables standard RFC7807 Problem Details responses
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();//assignemnt 4.3

builder.Services.AddSingleton<ListingStore>();

// Scalar (API docs UI)
builder.Services.AddOpenApi(); // required for Scalar in modern .NET templates

var app = builder.Build();

app.UseExceptionHandler(); // catches unhandled exceptions and returns Problem Details JSON
app.UseSerilogRequestLogging();//assignemnt 4.3

app.UseStatusCodePages(); // turns status codes like 404 into Problem Details responses

// Enable OpenAPI pipeline (Scalar uses this)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // exposes /openapi.json for Scalar
}
app.MapScalarApiReference();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();