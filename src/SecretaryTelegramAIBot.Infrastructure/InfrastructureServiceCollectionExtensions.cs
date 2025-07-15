using GenerativeAI;
using GenerativeAI.Web;
using Microsoft.EntityFrameworkCore;
using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Domain.Repositories;
using Telegram.Bot;
using GenerativeAIService = SecretaryTelegramAIBot.Infrastructure.Services.GenerativeAIService;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<ThirdPartiesOptions> configureOptions)
    {
        var settings = new ThirdPartiesOptions();
        configureOptions(settings);
        services.Configure(configureOptions);

        services.AddDbContext<TelegramBotDbContext>(options =>
        {
            options.UseSqlServer(settings.ConnectionStrings.Default);
        });
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ICommand).Assembly);
        });
        
        services.AddGenerativeAI(options =>
        {
            options.Credentials = new GoogleAICredentials(settings.GenerateAI.GeminiAPIKey);
            options.ProjectId = settings.GenerateAI.ProjectId;
            options.Model = settings.GenerateAI.Model;
        });
        
        services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient) => new TelegramBotClient(settings.Telegram.APIToken, httpClient));
        
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddScoped<GenerativeAIService>();
        
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddHostedServices();
        
        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<PollingService>();

        return services;
    }
}