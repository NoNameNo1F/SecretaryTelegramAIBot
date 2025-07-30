using GenerativeAI;
using GenerativeAI.Web;
using Microsoft.Extensions.DependencyInjection;
using SecretaryTelegramAIBot.Application.Services;
using GenerativeAIOptions = SecretaryTelegramAIBot.Infrastructure.ConfigurationOptions.GenerativeAIOptions;
using GenerativeAIService = SecretaryTelegramAIBot.Infrastructure.Services.GenerativeAIService;

namespace SecretaryTelegramAIBot.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<GenerativeAIOptions> configureOptions)
    {
        var settings = new GenerativeAIOptions();
        configureOptions(settings);
        services.Configure(configureOptions);
        
        services.AddGenerativeAI(options =>
        {
            options.Credentials = new GoogleAICredentials(settings.APIKey);
            options.ProjectId = settings.ProjectId;
            options.Model = settings.Model;
        });
        services.AddScoped<IGenerativeAIService, GenerativeAIService>();

        return services;
    }
}