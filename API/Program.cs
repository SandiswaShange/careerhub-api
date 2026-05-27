using API.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddSingleton<ListingStore>();

// Scalar (API docs UI)
builder.Services.AddOpenApi(); // required for Scalar in modern .NET templates

var app = builder.Build();

// Enable OpenAPI pipeline (Scalar uses this)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // exposes /openapi.json for Scalar
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();