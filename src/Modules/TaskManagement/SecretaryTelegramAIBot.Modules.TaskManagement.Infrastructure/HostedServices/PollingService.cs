using Microsoft.Extensions.Logging;

namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.HostedServices;
public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger) 
        : base(serviceProvider, logger)
    {
    }
}