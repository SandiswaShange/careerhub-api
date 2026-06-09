using Scalar.AspNetCore;
using API.Middleware;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Data;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();//assignemnt 4.3

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions());

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Host.UseSerilog();

// Controllers
builder.Services.AddControllers();

builder.Services.AddProblemDetails(); // enables standard RFC7807 Problem Details responses
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();//assignemnt 4.3

builder.Services.AddSingleton<SlowQueryInterceptor>();
builder.Services.AddDbContext<JobListingDbContext>((sp, options) =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"));

    options.AddInterceptors(
        sp.GetRequiredService<SlowQueryInterceptor>());
});

// Scalar (API docs UI)
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
    {
     options.AddPolicy("AuthorizationPolicy", policy =>
     {
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
     }); 
    });
    var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Not validating who issues it bc its our own API
            ValidateAudience = false, // Not checking who it is intended for
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey!)
            )
        };
    });
    builder.Services.AddAuthorization();
    builder.Services.AddRepositories();
    builder.Services.AddServices();

var app = builder.Build();


app.UseSerilogRequestLogging();//assignemnt 4.3
    app.UseCors("AuthorizationPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
app.UseExceptionHandler(); // catches unhandled exceptions and returns Problem Details JSON

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