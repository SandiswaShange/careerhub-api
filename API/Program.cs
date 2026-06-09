using Scalar.AspNetCore;
using API.Middleware;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Services;
using API.Data;
using Asp.Versioning;
using System.Threading.RateLimiting;    

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

builder.Services.AddRateLimitingPolicies();

builder.Services.AddProblemDetails(); // enables standard RFC7807 Problem Details responses
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();//assignemnt 4.3

builder.Services.AddDbContext<JobListingDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Scalar (API docs UI)
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
     options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://careerhub.example.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("X-Total-Count");
    })
    );
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

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);

        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ReportApiVersions = true;
    });

var app = builder.Build();


app.UseSerilogRequestLogging();//assignemnt 4.3
    app.UseCors("FrontendPolicy");
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

app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("global");

app.Run();