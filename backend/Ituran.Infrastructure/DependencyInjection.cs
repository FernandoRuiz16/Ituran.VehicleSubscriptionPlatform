using Ituran.Application.Interfaces;
using Ituran.Application.Services;
using Ituran.Infrastructure.Persistence;
using Ituran.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Ituran.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IWebhookRepository, WebhookRepository>();
        services.AddScoped<IProcessingLogRepository, ProcessingLogRepository>();

       

        return services;
    }
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<WebhookIngestionService>();
        services.AddScoped<VehicleSubscriptionProcessorService>();
        services.AddScoped<WebhookIngestionService>();

        return services;
    }
}