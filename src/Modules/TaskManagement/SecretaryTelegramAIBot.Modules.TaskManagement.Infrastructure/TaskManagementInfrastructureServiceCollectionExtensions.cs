using Microsoft.EntityFrameworkCore;
using SecretaryTelegramAIBot.Application.Contracts;
using SecretaryTelegramAIBot.Domain.Repositories;
using SecretaryTelegramAIBot.Modules.TaskManagement.Domain.Notes;
using Telegram.Bot;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TaskManagementInfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddTaskManagementInfrastructure(this IServiceCollection services, Action<ThirdPartiesOptions> configureOptions)
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
        
            
            services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
                .AddTypedClient<ITelegramBotClient>((httpClient)
                    => new TelegramBotClient(settings.Telegram.APIToken, httpClient));

            services.AddScoped<UpdateHandler>();
            services.AddScoped<ReceiverService>();
            
            
            services.AddScoped<TelegramBotService>();

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTaskManagementHostedServices();

            return services;
        }

        private static IServiceCollection AddTaskManagementHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<PollingService>();

            return services;
        }
    }
}