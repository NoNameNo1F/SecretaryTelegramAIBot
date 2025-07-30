namespace SecretaryTelegramAIBot.Modules.TaskManagement.Infrastructure.Abstractions;
public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}