using Iam.Application.Contracts.Logging;
using Iam.Application.Contracts.Notification;
using Iam.Application.Models;
using Iam.Infrastructure.Logging;
using Iam.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Iam.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

        if (apiSettings != null && apiSettings.NotificationUrl != null)
        {
            services.AddSingleton<INotificationService>(new NotificationService(apiSettings.NotificationUrl));
        }

        return services;
    }
}
