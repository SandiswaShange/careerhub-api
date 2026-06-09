using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace API.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IJobListingRepository,JobListingRepository>();
        services.AddScoped<ICompanyRepository,CompanyRepository>();
        services.AddScoped<IApplicationRepository,ApplicationRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IJobListingService,JobListingService>();
        services.AddScoped<IApplicationService,ApplicationService>();
        services.AddSingleton<ApplicationStatusRules>();
        services.AddSingleton<SlowQueryInterceptor>();
        return services;
    }
    
    public static IServiceCollection AddRateLimitingPolicies(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;

                if (context.Lease.TryGetMetadata(
                    MetadataName.RetryAfter,
                    out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString();

                    await context.HttpContext.Response.WriteAsync(
                        $"Rate limit exceeded. Please retry after {(int)retryAfter.TotalSeconds} seconds.",
                        token);
                }
            };

            options.AddFixedWindowLimiter("global", opt =>
            {
                opt.PermitLimit = 200;
                opt.Window = TimeSpan.FromSeconds(60);
                opt.QueueLimit = 0;
            });

            options.AddSlidingWindowLimiter("search", opt =>
            {
                opt.PermitLimit = 30;
                opt.Window = TimeSpan.FromSeconds(60);
                opt.SegmentsPerWindow = 6;
                opt.QueueLimit = 0;
            });

            options.AddFixedWindowLimiter("apply", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromHours(1);
                opt.QueueLimit = 0;
            });

            options.AddFixedWindowLimiter("post-listing", opt =>
            {
                opt.PermitLimit = 10;
                opt.Window = TimeSpan.FromHours(1);
                opt.QueueLimit = 0;
            });
        });

        return services;
    }
}