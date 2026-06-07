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
        return services;
    }
}